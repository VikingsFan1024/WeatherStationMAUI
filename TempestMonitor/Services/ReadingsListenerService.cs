using System.Diagnostics;
using TempestMonitor.Models;

namespace TempestMonitor.Services;

sealed public partial class ReadingsListenerService(IServiceProvider serviceProvider) : IDisposable
{
    void IDisposable.Dispose()
    {
        Stop();

        GC.SuppressFinalize(this);
    }

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private CancellationTokenSource? _cancellationTokenSource;

    private BufferBlockOfByteArray? _bufferBlockOfByteArray;
    private ActionBlockOfByteArray? _actionBlockOfByteArray;

    private ListOfTasks? _completionList;

    private DeviceStatusModel? _mostRecentDeviceStatusReading;
    public DeviceStatusModel? MostRecentDeviceStatusReading => _mostRecentDeviceStatusReading;
    private HubStatusModel? _mostRecentHubStatusReading;
    public HubStatusModel? MostRecentHubStatusReading => _mostRecentHubStatusReading;
    private LightningStrikeModel? _mostRecentLightningStrikeReading;
    public LightningStrikeModel? MostRecentLightningStrikeReading => _mostRecentLightningStrikeReading;
    private ObservationModel? _mostRecentObservationReading;
    public ObservationModel? MostRecentObservationReading => _mostRecentObservationReading;
    private RainStartModel? _mostRecentRainStartReading;
    public RainStartModel? MostRecentRainStartReading => _mostRecentRainStartReading;

    private DatabaseService databaseService = serviceProvider.GetRequiredService<DatabaseService>();

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
    private async TaskOfBool Init()
    {
        if (_cancellationTokenSource is null) return false;
        if (_completionList is null) return false;

        try
        {
            await Task.Run(() => SetupDataflow());

            _completionList.Add(Task.Run(() => ListenForStationUDPBroadcasts(), _cancellationTokenSource.Token));

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
    private async TaskOfBool ListenForStationUDPBroadcasts()
    {
        if (_bufferBlockOfByteArray is null) return false;
        if (_cancellationTokenSource is null) return false;

        UdpClient? udpClient = null;

        try
        {
            ApplicationStatisticsModel.StartOrResumeUdpStatistics();

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (udpClient is not null)
                {
                    udpClient.Close();
                    udpClient.Dispose();
                    udpClient = null;
                }

                udpClient = new(Constants.UdpListeningPort);

                Stopwatch stopwatch = new();
                stopwatch.Start();

                ValueTaskOfUdpReceiveResult valueTaskOfUdpReceiveResult;
                UdpReceiveResult udpReceiveResult;
                try
                {
                    valueTaskOfUdpReceiveResult = await Task.Run(
                        () => udpClient.ReceiveAsync(_cancellationTokenSource.Token));
                    udpReceiveResult = await valueTaskOfUdpReceiveResult;
                }

                catch (SocketException socketException)
                {
                    Log.Error(socketException, "SocketException, continuing loop");
                    continue;
                }

                catch (Exception exception)
                {
                    Log.Error(exception, "Exception in ReceiveAsync, exiting loop via re-throw");
                    throw;
                }

                stopwatch.Stop();

                if (!valueTaskOfUdpReceiveResult.IsCompleted)
                {
                    Log.Error("!valueTask.IsCompleted, exiting loop, returning false");
                    return false;
                }

                if (valueTaskOfUdpReceiveResult.IsCanceled)
                {
                    Log.Information("valueTask.IsCanceled, exiting loop, returning true");
                    return true;
                }

                if (valueTaskOfUdpReceiveResult.IsFaulted)
                {
                    Log.Warning("valueTask.IsFaulted, continuing loop");
                    continue;
                }

                if (!valueTaskOfUdpReceiveResult.IsCompletedSuccessfully)
                {
                    Log.Warning("!valueTask.IsCompletedSuccessfully, continuing loop");
                    continue;
                }

                if (udpReceiveResult.Buffer is null)
                {
                    Log.Information("udpReceiveResult.Buffer is null, continuing loop");
                    continue;
                }

                ApplicationStatisticsModel.SetLastUdpReading(stopwatch.ElapsedMilliseconds);

                var taskOfBool = _bufferBlockOfByteArray.SendAsync(udpReceiveResult.Buffer);

                taskOfBool.Wait(_cancellationTokenSource.Token);
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
            Log.Information(exception, $"OperationCanceledException, loop exited, returning {isOurRequestToCancel}");
            return isOurRequestToCancel;
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception, loop exited, re-throwing exception");
            throw;
        }

        finally
        {
            if (udpClient is not null)
            {
                udpClient.Close();
                udpClient.Dispose();
                udpClient = null;
            }

            Log.Information("Stopped listening for UDP broadcasts");

            ApplicationStatisticsModel.SetUdpBroadcastsBeingListenedForToFalse();
        }

        Log.Information("Exiting ListenForStationUDPBroadcasts due to cancellation, returning true");

        return true;
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

    public DatabaseBaseModel? CreateDatabaseBaseModelSubClass(byte[] byteArray)
    {

        if (byteArray is null || byteArray.Length == 0)
        {
            Log.Warning("Received null or empty byte array, ignoring");
            return null;
        }

        string? readingType = null;
        var utf8JsonReader = new System.Text.Json.Utf8JsonReader(byteArray);
        while (utf8JsonReader.Read())
        {
            Debug.Write(utf8JsonReader.TokenType);

            switch (utf8JsonReader.TokenType)
            {
                case System.Text.Json.JsonTokenType.StartObject:
                    break;

                case System.Text.Json.JsonTokenType.PropertyName:
                    if (utf8JsonReader.ValueTextEquals("type"))
                    {
                        utf8JsonReader.Read();
                        readingType = utf8JsonReader.GetString();
                    }
                    break;
            }

            if (readingType is not null) break;
        }

        switch (readingType)
        {
            case WindModel.type:
                return new WindModel() { json_document = byteArray };

            case ObservationModel.type:
                return new ObservationModel() { json_document = byteArray };

            case DeviceStatusModel.type:
                return new DeviceStatusModel() { json_document = byteArray };

            case HubStatusModel.type:
                return new HubStatusModel() { json_document = byteArray };

            case LightningStrikeModel.type:
                return new LightningStrikeModel() { json_document = byteArray };

            case RainStartModel.type:
                return new RainStartModel() { json_document = byteArray };

            default:
                Log.Warning($"Unknown reading type {readingType}");
                return null;
        }
    }
}