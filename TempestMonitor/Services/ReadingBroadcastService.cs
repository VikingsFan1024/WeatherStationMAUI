using Exception = System.Exception;          // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
using Task = System.Threading.Tasks.Task;    // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
namespace TempestMonitor.Services;

public class ReadingBroadcastService(IServiceProvider serviceProvider)
{
    private DatabaseService databaseService = serviceProvider.GetRequiredService<DatabaseService>();
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isRunning;
    private ListOfTasks? _completionList;
    public VW_WindModel? MostRecentVW_WindModel { get; private set; } = null;
    public VW_ObservationModel? MostRecentVW_ObservationModel { get; private set; } = null;
    public VW_DeviceStatusModel? MostRecentVW_DeviceStatusModel { get; private set; } = null;
    public WeatherForecastGraph? MostRecentVW_WeatherForecastModel { get; private set; } = null;

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
    private bool Init()
    {
        if (_cancellationTokenSource is null) return false;
        if (_completionList is null) return false;

        try
        {
            _completionList.Add(Task.Run(() => HandleWeakReferenceMessages(), _cancellationTokenSource.Token));

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

    private void HandleWeakReferenceMessages()
    {
        WeakReferenceMessenger.Default.Register
        (
            this,
            (CommunityToolkit.Mvvm.Messaging.MessageHandler<object, Models.DatabaseTableAndKeyMessage>)
            (
                (r, m) =>
                {
                    try
                    {
                        var tablenameRowId = m.TablenameRowId;
                        switch (tablenameRowId.TableName)
                        {
                            // ToDo: Find a way to get rid of these hard coded strings in the case statement
                            case "Wind":
                                var vw_WindModel = databaseService.GetReadingByRowId<VW_WindModel>(tablenameRowId.RowId);
                                ApplicationStatisticsModel.IncrementWindReadingReceivedCount();
                                MostRecentVW_WindModel = vw_WindModel;
                                WeakReferenceMessenger.Default.Send(new VW_Message<VW_WindModel>(vw_WindModel));
                                break;

                            case "Observation":
                                var vw_ObservationModel = databaseService.GetReadingByRowId<VW_ObservationModel>(tablenameRowId.RowId);
                                ApplicationStatisticsModel.IncrementObservationReadingReceivedCount();
                                MostRecentVW_ObservationModel = vw_ObservationModel;
                                WeakReferenceMessenger.Default.Send(new VW_Message<VW_ObservationModel>(vw_ObservationModel));
                                break;

                            case "DeviceStatus":
                                var vw_DeviceStatusModel = databaseService.GetReadingByRowId<VW_DeviceStatusModel>(tablenameRowId.RowId);
                                ApplicationStatisticsModel.IncrementDeviceStatusReceivedCount();
                                MostRecentVW_DeviceStatusModel = vw_DeviceStatusModel;
                                WeakReferenceMessenger.Default.Send(new VW_Message<VW_DeviceStatusModel>(vw_DeviceStatusModel));
                                break;

                            case "HubStatus":
                                var vw_HubStatusModel = databaseService.GetReadingByRowId<VW_HubStatusModel>(tablenameRowId.RowId);
                                ApplicationStatisticsModel.IncrementHubStatusReceivedCount();
                                WeakReferenceMessenger.Default.Send(new VW_Message<VW_HubStatusModel>(vw_HubStatusModel));
                                break;

                            case "LightningStrike":
                                var vw_LightningStrikeModel = databaseService.GetReadingByRowId<VW_LightningStrikeModel>(tablenameRowId.RowId);
                                ApplicationStatisticsModel.IncrementLightningStrikeReceivedCount();
                                WeakReferenceMessenger.Default.Send(new VW_Message<VW_LightningStrikeModel>(vw_LightningStrikeModel));
                                break;

                            case "RainStart":
                                var vw_RainStartModel = databaseService.GetReadingByRowId<VW_RainStartModel>(tablenameRowId.RowId);
                                ApplicationStatisticsModel.IncrementRainStartReceivedCount();
                                WeakReferenceMessenger.Default.Send(new VW_Message<VW_RainStartModel>(vw_RainStartModel));
                                break;

                            case "WeatherForecast":
                                var weatherForecastModel = databaseService.GetReadingByRowId<WeatherForecastModel>(tablenameRowId.RowId);
                                ApplicationStatisticsModel.IncrementForecastReceivedCount();
                                var weatherForecastGraph = JsonSerializer.Deserialize<Models.WeatherForecastGraph>(weatherForecastModel.json_document);
                                if (weatherForecastGraph is null)
                                {
                                    Log.Warning("vw_WeatherForecastModel is null");
                                    return;
                                }
                                MostRecentVW_WeatherForecastModel = weatherForecastGraph;
                                WeakReferenceMessenger.Default.Send(new VW_Message<WeatherForecastGraph>(weatherForecastGraph));
                                break;

                            default:
                                Log.Warning("Unknown table name: {TableName}", tablenameRowId.TableName);
                                break;
                        }
                    }

                    catch (Exception exception)
                    {
                        Log.Error(exception, $"Table[{m.TablenameRowId.TableName}] Id[{m.TablenameRowId.RowId}] Exception in HandleWeakReferenceMessages when reading inserted row to broadcast it, ignoring continuing");
                    }
                }
            )
        );
    }

    public void Stop()
    {
        if (!_isRunning)
        {
            Log.Information("Not running");
            return;
        }

        if (_cancellationTokenSource is null) Log.Information("cancellationTokenSource is null");

        _cancellationTokenSource?.Cancel();

        WeakReferenceMessenger.Default.UnregisterAll(this);

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

        _cancellationTokenSource = null;

        _isRunning = false;

        Log.Information("Stopped");
    }
}
