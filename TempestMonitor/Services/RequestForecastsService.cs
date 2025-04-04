// static using for extension method classes
using static CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;  // for Send method
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;
using static System.Net.Http.Json.HttpClientJsonExtensions; // for GetFromJsonAsync<T> extension method
using static System.Threading.Tasks.Dataflow.DataflowBlock; // for BufferBlock and ActionBlock methods - SendAsync()

// Aliases for types used in this file to keep the code cleaner
using ActionBlockOfForecastModel = System.Threading.Tasks.Dataflow.ActionBlock<TempestMonitor.Models.ForecastModel>;
using BufferBlockOfForecastModel = System.Threading.Tasks.Dataflow.BufferBlock<TempestMonitor.Models.ForecastModel>;
using ListOfTasks = System.Collections.Generic.List<System.Threading.Tasks.Task>;
using TaskOfBool = System.Threading.Tasks.Task<bool>;
using TaskOfNullableJsonDocument = System.Threading.Tasks.Task<System.Text.Json.JsonDocument?>;
using ValueChangedMessageOfForecastModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.ForecastModel>;
using ValueChangedMessageOfObject = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<object>;

// using directives for precision in what specific classes are employed
using AggregateException = System.AggregateException;
using ApplicationStatisticsModel = TempestMonitor.Models.ApplicationStatisticsModel;
using CancellationTokenSource = System.Threading.CancellationTokenSource;
using DataflowLinkOptions = System.Threading.Tasks.Dataflow.DataflowLinkOptions;
using Exception = System.Exception;
using ExecutionDataflowBlockOptions = System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions;
using ForecastChildModel = TempestMonitor.Models.ForecastChildModel;
using ForecastModel = TempestMonitor.Models.ForecastModel;
using HttpClient = System.Net.Http.HttpClient;
using HttpRequestException = System.Net.Http.HttpRequestException;
using IDisposable = System.IDisposable;
using IServiceProvider = System.IServiceProvider;
using JsonDocument = System.Text.Json.JsonDocument;
using Log = Serilog.Log;
using OperationCanceledException = System.OperationCanceledException;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using SettingsModel = TempestMonitor.Models.SettingsModel;
using Stopwatch = System.Diagnostics.Stopwatch;
using Task = System.Threading.Tasks.Task;
using TaskCanceledException = System.Threading.Tasks.TaskCanceledException;
using TimeSpan = System.TimeSpan;

namespace TempestMonitor.Services;
sealed public partial class RequestForecastsService(IServiceProvider serviceProvider) : IDisposable
{
    public class ForecastMessage(ForecastModel forecastModel) :
        ValueChangedMessageOfForecastModel(forecastModel)
    {
        private readonly ForecastModel _forecastModel = forecastModel;
        public ForecastModel Forecast => _forecastModel;
    }

    public class ForecastPartMessage(object forecastPart) :
        ValueChangedMessageOfObject(forecastPart)
    {
        private readonly object _forecastPart = forecastPart;
        public object ForecastPart => _forecastPart;
    }

    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private ForecastModel? _mostRecentForecast;
    void IDisposable.Dispose()
    {
        Stop();
    }

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private CancellationTokenSource? _cancellationTokenSource;

    private BufferBlockOfForecastModel? _forecastModelBufferBlock;
    private ActionBlockOfForecastModel? _forecastModelToUserInterfaceAndDatabaseBlock;

    private ListOfTasks? _completionList;
    private bool _isRunning;

    public ForecastModel? MostRecentForecast => _mostRecentForecast;

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

        _forecastModelBufferBlock =
            new BufferBlockOfForecastModel
            (
                new ExecutionDataflowBlockOptions
                {
                    NameFormat = nameof(_forecastModelBufferBlock),
                    BoundedCapacity = 1,
                    MaxMessagesPerTask = 1,
                    SingleProducerConstrained = true,
                    CancellationToken = _cancellationTokenSource.Token
                }
            );

        _forecastModelToUserInterfaceAndDatabaseBlock =
            new ActionBlockOfForecastModel
            (
                forecastModel =>
                {
                    _mostRecentForecast = new ForecastModel(forecastModel.JsonElement);

                    // Send to user interface
                    WeakReferenceMessenger.Default.Send(new ForecastMessage(forecastModel));

                    // Send to database
                    SendToDatabase(forecastModel);
                    SendToDatabase(forecastModel.CurrentConditions);
                    SendToDatabase(forecastModel.Station);
                    SendToDatabase(forecastModel.Status);
                    SendToDatabase(forecastModel.Units);
                    SendToDatabase(forecastModel.Dailies);
                    SendToDatabase(forecastModel.Hourlies);
                },
                new ExecutionDataflowBlockOptions
                {
                    NameFormat = nameof(_forecastModelBufferBlock),
                    BoundedCapacity = 1,
                    MaxMessagesPerTask = 1,
                    SingleProducerConstrained = true,
                    CancellationToken = _cancellationTokenSource.Token
                }
            );

        _forecastModelBufferBlock.LinkTo(
            _forecastModelToUserInterfaceAndDatabaseBlock,
            new DataflowLinkOptions { PropagateCompletion = true }
        );

        _completionList.AddRange
        (
            [
                _forecastModelBufferBlock.Completion,
                _forecastModelToUserInterfaceAndDatabaseBlock.Completion,
            ]
        );

        return true;
    }
    private static void SendToDatabase(object? forecastPart)
    {
        if (forecastPart is null) return;

        if (forecastPart is ForecastChildModel[] ForecastChildModelArray)
            foreach (var iForecastChildModelItem in ForecastChildModelArray)
                SendToDatabase(iForecastChildModelItem);
        else
            WeakReferenceMessenger.Default.Send(new ForecastPartMessage(forecastPart));
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
        if (_forecastModelBufferBlock is null) return false;
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

                TaskOfNullableJsonDocument? taskOfJsonDocument = null;
                JsonDocument? jsonDocument = null;
                try
                {
                    taskOfJsonDocument = Task.Run(
                        () => httpClient.GetFromJsonAsync<JsonDocument>(requestString, _cancellationTokenSource.Token));
                    jsonDocument = await taskOfJsonDocument;
                }

                catch(HttpRequestException exception)
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

                if (!taskOfJsonDocument.IsCompleted)
                {
                    Log.Warning("taskOfJsonDocument is not completed, exiting loop, returning false");
                    return false;
                }

                if (taskOfJsonDocument.IsCanceled)
                {
                    Log.Information("taskOfJsonDocument is canceled, exiting loop, returning true");
                    return true;
                }

                if (taskOfJsonDocument.IsFaulted)
                {
                    Log.Warning(taskOfJsonDocument.Exception, "taskOfJsonDocument is faulted, continuing loop");
                    continue;
                }

                if (!taskOfJsonDocument.IsCompletedSuccessfully)
                {
                    Log.Warning("taskOfJsonDocument is not completed successfully, continuing loop");
                    continue;
                }

                if (jsonDocument is null)
                {
                    Log.Error("jsonDocument is null, continuing loop");
                    continue;
                }

                ApplicationStatisticsModel.SetLastHttpResponse(stopwatch.ElapsedMilliseconds);

                _ = _forecastModelBufferBlock.SendAsync(new ForecastModel(jsonDocument.RootElement));

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