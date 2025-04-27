namespace TempestMonitor.Services;
sealed public partial class RequestForecastsService(IServiceProvider serviceProvider) : IDisposable
{
    private DatabaseService databaseService = serviceProvider.GetRequiredService<DatabaseService>();

    private readonly IServiceProvider _serviceProvider = serviceProvider;
    void IDisposable.Dispose()
    {
        Stop();
    }

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private CancellationTokenSource? _cancellationTokenSource;

    private BufferBlockOfByteArray? _bufferBlockOfByteArray;
    private ActionBlockOfByteArray? _actionBlockOfByteArray;

    private ListOfTasks? _completionList;
    private bool _isRunning;

    public void Start()
    {
        if (_isRunning)
        {
            Log.Information("Already running");
            return;
        }

        _cancellationTokenSource?.Dispose();
        _completionList?.Clear();

        _cancellationTokenSource = new();
        _completionList = [];

        Task.Run(() => Init());

        Log.Information("Started");
    }
    public async TaskOfBool Init()
    {
        if (_cancellationTokenSource is null) return false;
        if (_completionList is null) return false;

        if (!SettingsAreValid())
        {
            Log.Error("Settings are not valid");
            return false;
        }

        try
        {
            await Task.Run(() => SetupDataflow());

            _completionList.Add(Task.Run(() => RequestForecasts(), _cancellationTokenSource.Token));

            _isRunning = true;

            return true;
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception");
            return false;
        }
    }
    private bool SetupDataflow()
    {
        if (_cancellationTokenSource is null) return false;
        if (_completionList is null) return false;

        _bufferBlockOfByteArray = new
        (
            new DataflowBlockOptions
            {
                NameFormat = nameof(_bufferBlockOfByteArray),
                BoundedCapacity = 1,
                MaxMessagesPerTask = 1,
                CancellationToken = _cancellationTokenSource.Token
            }
        );

        _actionBlockOfByteArray = new
        (
            byteArray =>
            {
                try
                {
                    var databaseBaseModel = CreateDatabaseBaseModelSubClass(byteArray);

                    if (databaseBaseModel is null)
                    {
                        Log.Warning("Received null or empty byte array, ignoring");
                    }
                    else
                    {
                        databaseService.SaveBufferToDB(databaseBaseModel);
                    }
                }

                catch (Exception exception)
                {
                    Log.Error(exception, "Failed to parse byte array to databaseBaseModel");
                }
            },
            new ExecutionDataflowBlockOptions
            {
                NameFormat = nameof(_actionBlockOfByteArray),
                BoundedCapacity = 1,
                MaxMessagesPerTask = 1,
                SingleProducerConstrained = true,
                CancellationToken = _cancellationTokenSource.Token
            }
        );

        _bufferBlockOfByteArray.LinkTo(
            _actionBlockOfByteArray, new DataflowLinkOptions { PropagateCompletion = true });

        _completionList.AddRange
        (
            [
                _bufferBlockOfByteArray.Completion,
                _actionBlockOfByteArray.Completion,
            ]
        );

        return true;
    }
    public DatabaseBaseModel? CreateDatabaseBaseModelSubClass(byte[] byteArray)
    {
        return new WeatherForecastModel() { json_document = byteArray };
    }
    public void Stop()
    {
        if (!_isRunning)
        {
            Log.Information("Not running");
            return;
        }

        if (_cancellationTokenSource is null) Log.Information("cancellationTokenSource is null");
        if (_completionList is null) Log.Information("completionList is null");

        _cancellationTokenSource?.Cancel();  //Possible ObjectDisposedException, AggregateException

        if (_completionList is not null)
        {
            foreach (var task in _completionList)
            {
                try
                {
                    if (!task.IsCanceled) task.Wait();
                }

                catch (AggregateException aggregateException)
                {
                    if (aggregateException.InnerException is TaskCanceledException)
                    {
                        Log.Information("TaskCanceledException in AggregateException, ignoring and continuing stop");
                        continue;
                    }
                    else
                    {
                        Log.Error(aggregateException, "Unexpected AggregateException");
                        break;
                    }
                }

                catch (Exception exception)
                {
                    Log.Error(exception, "Unrecognized exception");
                    break;
                }
            }
        }

        _cancellationTokenSource?.Dispose();
        _completionList?.Clear();

        _cancellationTokenSource = null;
        _completionList = null;

        _isRunning = false;

        Log.Information("Stopped");
    }
    public async TaskOfBool RequestForecasts()
    {
        if (_bufferBlockOfByteArray is null) return false;
        if (_cancellationTokenSource is null) return false;

        HttpClient? httpClient = null;

        try
        {
            ApplicationStatisticsModel.StartOrResumeHttpStatistics();

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (httpClient is not null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                    httpClient = new HttpClient();
                }

                httpClient = new HttpClient();

                Stopwatch stopwatch = new();
                stopwatch.Start();
                var requestString = $"{Constants.BaseForecastURL}?station_id={_settings.StationID}&token={_settings.RestAPIKey}";

                TaskOfByteArray? taskOfByteArray = null;
                byte[]? byteArray = null;
                try
                {
                    taskOfByteArray = Task.Run(
                        () => httpClient.GetByteArrayAsync(requestString, _cancellationTokenSource.Token));
                    byteArray = await taskOfByteArray;
                }

                catch (HttpRequestException exception)
                {
                    Log.Error(exception, "HttpRequestException in GetFromJsonAsync, continuing loop");
                    continue;
                }

                catch (Exception exception)
                {
                    Log.Error(exception, "Exception in GetFromJsonAsync, exiting loop via throw");
                    throw;
                }

                stopwatch.Stop();

                if (!taskOfByteArray.IsCompleted)
                {
                    Log.Warning("taskOfJsonDocument is not completed, exiting loop, returning false");
                    return false;
                }

                if (taskOfByteArray.IsCanceled)
                {
                    Log.Information("taskOfJsonDocument is canceled, exiting loop, returning true");
                    return true;
                }

                if (taskOfByteArray.IsFaulted)
                {
                    Log.Warning(taskOfByteArray.Exception, "taskOfJsonDocument is faulted, continuing loop");
                    continue;
                }

                if (!taskOfByteArray.IsCompletedSuccessfully)
                {
                    Log.Warning("taskOfJsonDocument is not completed successfully, continuing loop");
                    continue;
                }

                if (byteArray is null)
                {
                    Log.Error("byteArray is null, continuing loop");
                    continue;
                }

                ApplicationStatisticsModel.SetLastHttpResponse(stopwatch.ElapsedMilliseconds);

                _ = _bufferBlockOfByteArray.SendAsync(byteArray);

                _cancellationTokenSource.Token.WaitHandle.WaitOne(
                    TimeSpan.FromMinutes(_settings.TimeBetweenHttpRequestsInMinutes));
            }
        }

        catch (TaskCanceledException exception)
        {
            bool isOurRequestToCancel = _cancellationTokenSource.Token.IsCancellationRequested;
            Log.Information(exception, $"TaskCanceledException, loop exited, returning {isOurRequestToCancel}");
            return isOurRequestToCancel;
        }

        catch (OperationCanceledException exception)
        {
            bool isOurRequestToCancel = _cancellationTokenSource.Token.IsCancellationRequested;
            Log.Information(exception, $"TaskCanceledException, loop exited, returning {isOurRequestToCancel}");
            return isOurRequestToCancel;
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception, loop exited, re-throwing exception");
            throw;
        }

        finally
        {
            if (httpClient is not null)
            {
                httpClient.Dispose();
                httpClient = null;
                httpClient = new HttpClient();
            }

            Log.Information("Stopped making Http Requests");

            ApplicationStatisticsModel.SetHttpRequestsBeingMadeToFalse();
        }

        Log.Information("Exiting RequestForecasts due to cancellation, returning true");

        return true;
    }
    public bool SettingsAreValid()
    {
        if (string.IsNullOrWhiteSpace(_settings.StationID))
        {
            Log.Error("StationID is null or empty");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_settings.RestAPIKey))
        {
            Log.Error("RestAPIKey is null or empty");
            return false;
        }

        return true;
    }
}