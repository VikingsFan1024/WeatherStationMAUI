// static using for extension method classes
using static CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;  // for Send method
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;
using static System.Threading.Tasks.Dataflow.DataflowBlock; // for BufferBlock and ActionBlock methods - SendAsync()

// Aliases for types used in this file to keep the code cleaner
using ActionBlockOfReadingModel = System.Threading.Tasks.Dataflow.ActionBlock<TempestMonitor.Models.ReadingModel?>;
using TaskOfBool = System.Threading.Tasks.Task<bool>;
using TransformBlockOfByteArrayToReadingModel = System.Threading.Tasks.Dataflow.TransformBlock<byte[], TempestMonitor.Models.ReadingModel?>;
using ValueChangedMessageOfAirObservationModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.AirObservationModel>;
using ValueChangedMessageOfHubStatusModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.HubStatusModel>;
using ValueChangedMessageOfLightningStrikeModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.LightningStrikeModel>;
using ValueChangedMessageOfObservationModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.ObservationModel>;
using ValueChangedMessageOfReadingModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.ReadingModel>;
using ValueChangedMessageOfRainStartModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.RainStartModel>;
using ValueChangedMessageOfSkyObservationModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.SkyObservationModel>;
using ValueChangedMessageOfWindReadingModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.WindReadingModel>;
using ValueChangedMessageOfDeviceStatusModel = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.DeviceStatusModel>;

// using directives for precision in what specific classes are employed
using AggregateException = System.AggregateException;
using ApplicationStatisticsModel = TempestMonitor.Models.ApplicationStatisticsModel;
using AirObservationModel = TempestMonitor.Models.AirObservationModel;
using CancellationTokenSource = System.Threading.CancellationTokenSource;
using DeviceStatusModel = TempestMonitor.Models.DeviceStatusModel;
using DataflowBlock = System.Threading.Tasks.Dataflow.DataflowBlock;
using DataflowLinkOptions = System.Threading.Tasks.Dataflow.DataflowLinkOptions;
using Exception = System.Exception;
using ExecutionDataflowBlockOptions = System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions;
using GC = System.GC;
using HubStatusModel = TempestMonitor.Models.HubStatusModel;
using IDisposable = System.IDisposable;
using IServiceProvider = System.IServiceProvider;
using JsonDocument = System.Text.Json.JsonDocument;
using LightningStrikeModel = TempestMonitor.Models.LightningStrikeModel;
using ListOfTasks = System.Collections.Generic.List<System.Threading.Tasks.Task>;
using Log = Serilog.Log;
using ObservationModel = TempestMonitor.Models.ObservationModel;
using OperationCanceledException = System.OperationCanceledException;
using RainStartModel = TempestMonitor.Models.RainStartModel;
using ReadingModel = TempestMonitor.Models.ReadingModel;
using SettingsModel = TempestMonitor.Models.SettingsModel;
using SkyObservationModel = TempestMonitor.Models.SkyObservationModel;
using SocketException = System.Net.Sockets.SocketException;
using Stopwatch = System.Diagnostics.Stopwatch;
using Task = System.Threading.Tasks.Task;
using TaskCanceledException = System.Threading.Tasks.TaskCanceledException;
using UdpClient = System.Net.Sockets.UdpClient;
using UdpReceiveResult = System.Net.Sockets.UdpReceiveResult;
using ValueTaskOfUdpReceiveResult = System.Threading.Tasks.ValueTask<System.Net.Sockets.UdpReceiveResult>;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using WindReadingModel = TempestMonitor.Models.WindReadingModel;
using System.Text.Json;

namespace TempestMonitor.Services;

sealed public partial class ReadingsListenerService(IServiceProvider serviceProvider) : IDisposable
{
    public class WindReadingMessage(WindReadingModel windReading)
        : ValueChangedMessageOfWindReadingModel(windReading)
    {
        private readonly WindReadingModel _windReading = windReading;
        public WindReadingModel WindReading => _windReading;
    }
    public class ObservationReadingMessage(ObservationModel observationReading) :
        ValueChangedMessageOfObservationModel(observationReading)
    {
        private readonly ObservationModel _observationReading = observationReading;
        public ObservationModel ObservationReading => _observationReading;
    }
    public class LightningStrikeMessage(LightningStrikeModel lightningStrikeReading) :
        ValueChangedMessageOfLightningStrikeModel(lightningStrikeReading)
    {
        private readonly LightningStrikeModel _lightningStrikeReading = lightningStrikeReading;
        public LightningStrikeModel LightningStrikeReading => _lightningStrikeReading;
    }
    public class RainStartMessage(RainStartModel rainStartReading) :
        ValueChangedMessageOfRainStartModel(rainStartReading)
    {
        private readonly RainStartModel _rainStartReading = rainStartReading;
        public RainStartModel RainStartReading => _rainStartReading;
    }
    public class HubStatusMessage(HubStatusModel hubStatusReading) :
        ValueChangedMessageOfHubStatusModel(hubStatusReading)
    {
        private readonly HubStatusModel _hubStatusReading = hubStatusReading;
        public HubStatusModel HubStatusReading => _hubStatusReading;
    }
    public class DeviceStatusMessage(DeviceStatusModel deviceStatusReading) :
        ValueChangedMessageOfDeviceStatusModel(deviceStatusReading)
    {
        private readonly DeviceStatusModel _deviceStatusReading = deviceStatusReading;
        public DeviceStatusModel DeviceStatusReading => _deviceStatusReading;
    }
    public class AirObservationMessage(AirObservationModel airObservationReading) :
        ValueChangedMessageOfAirObservationModel(airObservationReading)
    {
        private readonly AirObservationModel _airObservationReading = airObservationReading;
        public AirObservationModel AirObservationReading => _airObservationReading;
    }
    public class SkyObservationMessage(SkyObservationModel skyObservationReading) :
        ValueChangedMessageOfSkyObservationModel(skyObservationReading)
    {
        private readonly SkyObservationModel _skyObservationReading = skyObservationReading;
        public SkyObservationModel SkyObservationReading => _skyObservationReading;
    }
    public class ReadingMessage(ReadingModel reading) : 
        ValueChangedMessageOfReadingModel(reading)
    {
        private readonly object _reading = reading;
        public object Reading => _reading;
    }

    void IDisposable.Dispose()
    {
        Stop();

        GC.SuppressFinalize(this);
    }

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private CancellationTokenSource? _cancellationTokenSource;

    private TransformBlockOfByteArrayToReadingModel? _udpReadingToReadingBlock;
    private ActionBlockOfReadingModel? _readingToReferenceMessagesBlock;

    private ListOfTasks? _completionList;

    private AirObservationModel? _mostRecentAirObservationReading;
    public AirObservationModel? MostRecentAirObservationReading => _mostRecentAirObservationReading;
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
    private SkyObservationModel? _mostRecentSkyObservationReading;
    public SkyObservationModel? MostRecentSkyObservationReading => _mostRecentSkyObservationReading;
    private WindReadingModel? _mostRecentWindReading;
    public WindReadingModel? MostRecentWindReading => _mostRecentWindReading;

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

        _udpReadingToReadingBlock =
            new TransformBlockOfByteArrayToReadingModel
            (
                // ToDo: Consider using a JsonSerializerOptions to optimize parsing for performance
                // ToDo: Consider using a custom JsonConverter for ReadingModel to handle deserialization directly
                // ToDo: Consider using a custom factory method to create ReadingModel instances based on the type in the JSON to avoid unnecessary allocations
                // ToDo: Consider using a caching mechanism for the JsonDocument to avoid multiple allocations in high throughput scenarios
                // ToDo: Consider using a memory pool for byte arrays to avoid allocations in high throughput scenarios
                // ToDo: Consider using a custom memory pool for byte arrays to avoid allocations in high throughput scenarios
                // ToDo: Consider using a custom JsonConverter for ReadingModel to handle deserialization directly
                byteArray => {
                    // Handle null or empty byte array gracefully
                    try
                    {
                        if (byteArray is null || byteArray.Length == 0)
                        {
                            Log.Warning("Received null or empty byte array, ignoring");
                            return null; // Return null to indicate failure to parse
                        }

                        // The constructor of ReadingModel precludes use of using the more performant JsonSerializer.Deserialize<ReadingModel>(byteArray);
                        var jsonDocument = JsonDocument.Parse(byteArray);

                        return new ReadingModel(jsonDocument.RootElement);
                    }

                    catch (Exception exception)
                    {
                        Log.Error(exception, "Failed to parse byte array to ReadingModel");
                        // To avoid crashing the pipeline, return null or handle accordingly
                        return null;
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    NameFormat = nameof(_udpReadingToReadingBlock),
                    BoundedCapacity = 1,
                    MaxMessagesPerTask = 1,
                    SingleProducerConstrained = true,
                    CancellationToken = _cancellationTokenSource.Token
                }
            );

        _readingToReferenceMessagesBlock =
            new ActionBlockOfReadingModel
            (
                readingModel =>
                {
                    try
                    {
                        if (readingModel is null)
                        {
                            Log.Warning("Received null reading in ActionBlock, ignoring");
                            return;
                        }
                        Send(readingModel);
                    }

                    catch (Exception exception)
                    {
                        Log.Error(exception, "Failed to send reading message");
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    NameFormat = nameof(_readingToReferenceMessagesBlock),
                    BoundedCapacity = 1,
                    MaxMessagesPerTask = 1,
                    CancellationToken = _cancellationTokenSource.Token
                }
            );

        _udpReadingToReadingBlock.LinkTo(
            _readingToReferenceMessagesBlock,
            new DataflowLinkOptions { PropagateCompletion = true }
        );

        _udpReadingToReadingBlock.LinkTo(DataflowBlock.NullTarget<ReadingModel?>());

        _completionList.AddRange
        (
            [
                _udpReadingToReadingBlock.Completion,
                _readingToReferenceMessagesBlock.Completion
            ]
        );

        return true;
    }
    private async TaskOfBool ListenForStationUDPBroadcasts()
    {
        if (_udpReadingToReadingBlock is null) return false;
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

                var taskOfBool = _udpReadingToReadingBlock.SendAsync(udpReceiveResult.Buffer);

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
    private void Send(ReadingModel? reading)
    {
        if (reading is null)
        {
            Log.Warning("Received null reading, ignoring");
            return;
        }

        ReadingModel? theReading;

        if (reading.Type == WindReadingModel.TypeName)
        {
            theReading = _mostRecentWindReading = new WindReadingModel(reading);
            ApplicationStatisticsModel.IncrementWindReadingReceivedCount();
            WeakReferenceMessenger.Default.Send(
                new WindReadingMessage(_mostRecentWindReading));
        }
        else if (reading.Type == ObservationModel.TypeName)
        {
            theReading = _mostRecentObservationReading = new ObservationModel(reading);
            ApplicationStatisticsModel.IncrementObservationReadingReceivedCount();

            WeakReferenceMessenger.Default.Send(
                new ObservationReadingMessage(_mostRecentObservationReading));
        }
        else if (reading.Type == LightningStrikeModel.TypeName)
        {
            theReading = _mostRecentLightningStrikeReading = new LightningStrikeModel(reading);
            ApplicationStatisticsModel.IncrementLightningStrikeReceivedCount();
            WeakReferenceMessenger.Default.Send(
                new LightningStrikeMessage(_mostRecentLightningStrikeReading));
        }
        else if (reading.Type == RainStartModel.TypeName)
        {
            theReading = _mostRecentRainStartReading = new RainStartModel(reading);
            ApplicationStatisticsModel.IncrementRainStartReceivedCount();
            WeakReferenceMessenger.Default.Send(
                new RainStartMessage(_mostRecentRainStartReading));
        }
        else if (reading.Type == HubStatusModel.TypeName)
        {
            theReading = _mostRecentHubStatusReading = new HubStatusModel(reading);
            ApplicationStatisticsModel.IncrementHubStatusReceivedCount();
            WeakReferenceMessenger.Default.Send(
                new HubStatusMessage(_mostRecentHubStatusReading));
        }
        else if (reading.Type == DeviceStatusModel.TypeName)
        {
            theReading = _mostRecentDeviceStatusReading = new DeviceStatusModel(reading);
            ApplicationStatisticsModel.IncrementDeviceStatusReceivedCount();
            WeakReferenceMessenger.Default.Send(
                new DeviceStatusMessage(_mostRecentDeviceStatusReading));
        }
        else if (reading.Type == AirObservationModel.TypeName)
        {
            theReading = _mostRecentAirObservationReading = new AirObservationModel(reading);
            ApplicationStatisticsModel.IncrementAirObservationReceivedCount();
            WeakReferenceMessenger.Default.Send(
                new AirObservationMessage(_mostRecentAirObservationReading));
        }
        else if (reading.Type == SkyObservationModel.TypeName)
        {
            theReading = _mostRecentSkyObservationReading = new SkyObservationModel(reading);
            ApplicationStatisticsModel.IncrementSkyObservationReceivedCount();
            WeakReferenceMessenger.Default.Send(
                new SkyObservationMessage(_mostRecentSkyObservationReading));
        }
        else
        {
            Log.Information($"Unknown reading type {reading.Type}");
            Log.Information($"Json {reading.JsonElementString}");
            return;
        }

        if (theReading is null) return;

        WeakReferenceMessenger.Default.Send(new ReadingMessage(theReading));
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
}