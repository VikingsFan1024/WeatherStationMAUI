using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;
using IDisposable = System.IDisposable;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Serilog;
using UdpReceiveResult = System.Net.Sockets.UdpReceiveResult;
using UdpClient = System.Net.Sockets.UdpClient;
using SocketException = System.Net.Sockets.SocketException; // for SocketException in ListenForStationUDPBroadcasts method
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using TempestMonitor.Models;
using System.Diagnostics;
using ApplicationStatisticsModel = TempestMonitor.Models.ApplicationStatisticsModel;
using System.Collections;
using TempestMonitor;
//using System.Net.WebSockets;
using Microsoft.Maui;
using System.Threading;
using Exception = System.Exception; // for Exception in ListenForStationUDPBroadcasts method
using OperationCanceledException = System.OperationCanceledException; // for OperationCanceledException in ListenForStationUDPBroadcasts method
using TaskCanceledException = System.Threading.Tasks.TaskCanceledException; // for TaskCanceledException in ListenForStationUDPBroadcasts method
using ValueTaskOfUdpReceiveResult = System.Threading.Tasks.ValueTask<System.Net.Sockets.UdpReceiveResult>;
using TaskOfBool = System.Threading.Tasks.Task<bool>; // for Task<bool> in SendClassInstanceToProcessing method

using ListOfTasks = System.Collections.Generic.List<System.Threading.Tasks.Task>;
using GC = System.GC; // for GC.SuppressFinalize(this) in Dispose method
using Task = System.Threading.Tasks.Task;
using AggregateException = System.AggregateException; // for AggregateException in Stop method

namespace TempestMonitor.Services;

sealed public partial class ReadingsListenerService(IServiceProvider serviceProvider) : IDisposable
{
    public class WindReadingMessage(WindReadingModel windReading)
        : ValueChangedMessage<WindReadingModel>(windReading)
    {
        private readonly WindReadingModel _windReading = windReading;
        public WindReadingModel WindReading => _windReading;
    }
    public class ObservationReadingMessage(ObservationModel observationReading) :
        ValueChangedMessage<ObservationModel>(observationReading)
    {
        private readonly ObservationModel _observationReading = observationReading;
        public ObservationModel ObservationReading => _observationReading;
    }
    public class LightningStrikeMessage(LightningStrikeModel lightningStrikeReading) :
        ValueChangedMessage<LightningStrikeModel>(lightningStrikeReading)
    {
        private readonly LightningStrikeModel _lightningStrikeReading = lightningStrikeReading;
        public LightningStrikeModel LightningStrikeReading => _lightningStrikeReading;
    }
    public class RainStartMessage(RainStartModel rainStartReading) :
        ValueChangedMessage<RainStartModel>(rainStartReading)
    {
        private readonly RainStartModel _rainStartReading = rainStartReading;
        public RainStartModel RainStartReading => _rainStartReading;
    }
    public class HubStatusMessage(HubStatusModel hubStatusReading) :
        ValueChangedMessage<HubStatusModel>(hubStatusReading)
    {
        private readonly HubStatusModel _hubStatusReading = hubStatusReading;
        public HubStatusModel HubStatusReading => _hubStatusReading;
    }
    public class DeviceStatusMessage(DeviceStatusModel deviceStatusReading) :
        ValueChangedMessage<DeviceStatusModel>(deviceStatusReading)
    {
        private readonly DeviceStatusModel _deviceStatusReading = deviceStatusReading;
        public DeviceStatusModel DeviceStatusReading => _deviceStatusReading;
    }
    public class AirObservationMessage(AirObservationModel airObservationReading) :
        ValueChangedMessage<AirObservationModel>(airObservationReading)
    {
        private readonly AirObservationModel _airObservationReading = airObservationReading;
        public AirObservationModel AirObservationReading => _airObservationReading;
    }
    public class SkyObservationMessage(SkyObservationModel skyObservationReading) :
        ValueChangedMessage<SkyObservationModel>(skyObservationReading)
    {
        private readonly SkyObservationModel _skyObservationReading = skyObservationReading;
        public SkyObservationModel SkyObservationReading => _skyObservationReading;
    }
    public class ReadingMessage(ReadingModel reading) : 
        ValueChangedMessage<ReadingModel>(reading)
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

    private TransformBlock<byte[], ReadingModel>? _udpReadingToReadingBlock;
    private ActionBlock<ReadingModel>? _readingToReferenceMessagesBlock;

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
            new TransformBlock<byte[], ReadingModel>
            (
                byteArray => new ReadingModel(JsonDocument.Parse(byteArray).RootElement.Clone()),
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
            new ActionBlock<ReadingModel>
            (
                readingModel => Send(readingModel),
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

        _udpReadingToReadingBlock.LinkTo(DataflowBlock.NullTarget<ReadingModel>());

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
    private void Send(ReadingModel reading)
    {
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