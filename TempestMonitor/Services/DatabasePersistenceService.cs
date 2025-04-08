namespace TempestMonitor.Services;
sealed public partial class DatabasePersistenceService(IServiceProvider serviceProvider) : IDisposable
{
    void IDisposable.Dispose()
    {
        Stop();
    }

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly DatabaseService _databaseService = serviceProvider.GetRequiredService<DatabaseService>();

    private CancellationTokenSource? _cancellationTokenSource;

    private BufferBlockOfObject? _classInstanceToProcess;
    private ActionBlockOfObject? _classInstanceToDatabase;

    private ListOfTasks? _completionList;
    private bool _isRunning;
    private readonly static string[] _tableNames =
    [
        "AirObservation",
        "CurrentConditions",
        "Daily",
        "DeviceStatus",
        "Forecast",
        "Hourly",
        "HubStatus",
        "LightningStrike",
        "Observation",
        "RainStart",
        "SkyObservation",
        "Station",
        "Status",
        "Units",
        "Wind"
    ];

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

        if (!EnsureReadyForProcessing()) return;

        Task.Run(() => Init());

        Log.Information("Started");
    }
    public bool EnsureReadyForProcessing()
    {
        try
        {
            using var databaseConnection = new SQLiteConnection(_settings.DatabaseFilename);

            foreach (var tableName in _tableNames)
            {
                var filename = $"SQLQueries\\Tables\\{tableName}.sql";
                var stream = FileSystem.OpenAppPackageFileAsync(filename).GetAwaiter().GetResult();
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    var contents = reader.ReadToEnd();
                    reader.Close();

                    try
                    {
                        databaseConnection.Execute(contents);
                    }

                    catch (Exception exception)
                    {
                        if (!exception.Message.Contains("already exists"))
                        {
                            Log.Information(exception, $"Exception creating {tableName}");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception");
            return false;
        }
    }
    public async TaskOfBool Init()
    {
        if (_cancellationTokenSource is null) return false;
        if (_completionList is null) return false;

        try
        {
            await Task.Run(() => SetupDataflow());

            WeakReferenceMessenger.Default.Register<RequestForecastsService.ForecastPartMessage>
            (
                this, (r, m) => { SendClassInstanceToProcessing(m.ForecastPart); }
            );

            WeakReferenceMessenger.Default.Register<ReadingsListenerService.ReadingMessage>
            (
                this, (r, m) => { SendClassInstanceToProcessing(m.Reading); }
            );

            _isRunning = true;

            _cancellationTokenSource.Token.WaitHandle.WaitOne();

            return true;
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception");
            return false;
        }
    }
    public bool SetupDataflow()
    {
        if (_cancellationTokenSource is null) return false;
        if (_completionList is null) return false;

        _classInstanceToProcess = new 
        (
            new DataflowBlockOptions
            {
                NameFormat = nameof(_classInstanceToProcess),
                BoundedCapacity = 1,
                MaxMessagesPerTask = 1,
                CancellationToken = _cancellationTokenSource.Token
            }
        );

        _classInstanceToDatabase = new 
        (
            classInstance =>
            {
                try
                {
                    _databaseService.SaveBufferToDB(classInstance);
                    AdvanceInstanceToDatabaseCounter(classInstance);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Failed to save class instance to database: {classInstanceType}", classInstance.GetType());
                }
            },
            new ExecutionDataflowBlockOptions
            {
                NameFormat = nameof(_classInstanceToDatabase),
                BoundedCapacity = 1,
                MaxMessagesPerTask = 1,
                SingleProducerConstrained = true,
                CancellationToken = _cancellationTokenSource.Token
            }
        );

        _classInstanceToProcess.LinkTo(
            _classInstanceToDatabase, new DataflowLinkOptions { PropagateCompletion = true });

        return true;
    }

    private static void AdvanceInstanceToDatabaseCounter(object classInstance)
    {
        // ToDo: Refactor this method to use a dictionary or similar structure to avoid the long if-else chain
        if (classInstance is ForecastModel)
            ApplicationStatisticsModel.IncrementForecastsSavedToDatabaseCount();
        else if (classInstance is StationModel)
            ApplicationStatisticsModel.IncrementForecastStationsSavedToDatabaseCount();
        else if (classInstance is StatusModel)
            ApplicationStatisticsModel.IncrementForecastStatusesSavedToDatabaseCount();
        else if (classInstance is UnitsModel)
            ApplicationStatisticsModel.IncrementForecastUnitsSavedToDatabaseCount();
        else if (classInstance is CurrentConditionsModel)
            ApplicationStatisticsModel.IncrementForecastCurrentConditionsSavedToDatabaseCount();
        else if (classInstance is DailyModel)
            ApplicationStatisticsModel.IncrementForecastDailiesSavedToDatabaseCount();
        else if (classInstance is HourlyModel)
            ApplicationStatisticsModel.IncrementForecastHourliesSavedToDatabaseCount();
        else if (classInstance is AirObservationModel)
            ApplicationStatisticsModel.IncrementAirObservationSavedToDatabaseCount();
        else if (classInstance is DeviceStatusModel)
            ApplicationStatisticsModel.IncrementDeviceStatusSavedToDatabaseCount();
        else if (classInstance is HubStatusModel)
            ApplicationStatisticsModel.IncrementHubStatusSavedToDatabaseCount();
        else if (classInstance is LightningStrikeModel)
            ApplicationStatisticsModel.IncrementLightningStrikeSavedToDatabaseCount();
        else if (classInstance is ObservationModel)
            ApplicationStatisticsModel.IncrementObservationReadingSavedToDatabaseCount();
        else if (classInstance is RainStartModel)
            ApplicationStatisticsModel.IncrementRainStartSavedToDatabaseCount();
        else if (classInstance is SkyObservationModel)
            ApplicationStatisticsModel.IncrementSkyObservationSavedToDatabaseCount();
        else if (classInstance is WindReadingModel)
            ApplicationStatisticsModel.IncrementWindReadingSavedToDatabaseCount();
        else
            Log.Error("Unknown class instance type: {classInstance}", classInstance.GetType());
    }
    private void SendClassInstanceToProcessing(object _classInstance)
    {
        _classInstanceToProcess?.SendAsync(_classInstance)
            .ContinueWith(
                (TaskOfBool task) =>
                {
                    if (task.IsFaulted)
                        Log.Error(task.Exception, "SendClassInstanceToProcessing");
                }
            );
    }
    public void Stop()
    {
        if (!_isRunning)
        {
            Log.Information("Not running");
            return;
        }

        try
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Unregistering with WeakReferenceMessenger in Stop");
        }

        if (_cancellationTokenSource is null) Log.Information("cancellationTokenSource is null");
        if (_completionList is null) Log.Information("completionList is null");

        _cancellationTokenSource?.Cancel();

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

                    Log.Error(aggregateException, "Unexpected AggregateException");
                    break;
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
}