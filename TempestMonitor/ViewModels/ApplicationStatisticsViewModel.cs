namespace TempestMonitor.ViewModels;
sealed partial class ApplicationStatisticsViewModel(IServiceProvider serviceProvider) : 
    INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private Timer? _timer;
    public void Dispose()
    {
        _timer?.Dispose();
        _timer = null;
    }
    public void OnDisappearing()
    {
        _foregroundServiceHandler?.Unregister(this);
        _timer?.Dispose();
        _timer = null;
    }
    public void OnAppearing()
    {
        _foregroundServiceHandler?.Register(this);
        _timer = new Timer(new TimerCallback(UpdateStatistics), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    public long DeviceStatusReceivedCount => ApplicationStatisticsModel.DeviceStatusReceivedCount;
    public long ForecastReceivedCount => ApplicationStatisticsModel.ForecastReceivedCount;
    public long HubStatusReceivedCount => ApplicationStatisticsModel.HubStatusReceivedCount;
    public long LightningStrikeReceivedCount => ApplicationStatisticsModel.LightningStrikeReceivedCount;
    public long ObservationReadingReceivedCount => ApplicationStatisticsModel.ObservationReadingReceivedCount;
    public long RainStartReceivedCount => ApplicationStatisticsModel.RainStartReceivedCount;
    public long WindReadingReceivedCount => ApplicationStatisticsModel.WindReadingReceivedCount;

    public DateTime? LastDeviceStatusReceivedDateTime => ApplicationStatisticsModel.LastDeviceStatusReceivedDateTime;
    public DateTime? LastForecastReceivedDateTime => ApplicationStatisticsModel.LastForecastReceivedDateTime;
    public DateTime? LastHubStatusReceivedDateTime => ApplicationStatisticsModel.LastHubStatusReceivedDateTime;
    public DateTime? LastLightningStrikeReceivedDateTime => ApplicationStatisticsModel.LastLightningStrikeReceivedDateTime;
    public DateTime? LastObservationReadingReceivedDateTime => ApplicationStatisticsModel.LastObservationReadingReceivedDateTime;
    public DateTime? LastRainStartReceivedDateTime => ApplicationStatisticsModel.LastRainStartReceivedDateTime;
    public DateTime? LastWindReadingReceivedDateTime => ApplicationStatisticsModel.LastWindReadingReceivedDateTime;

    public DateTime CurrentTime => DateTime.Now;

    public long UdpReadingCount => ApplicationStatisticsModel.UdpReadingCount;
    public long HttpResponseCount => ApplicationStatisticsModel.HttpResponseCount;
    public DateTime? LastUdpReadingDateTime => ApplicationStatisticsModel.LastUdpReadingDateTime;
    public DateTime? LastHttpResponseDateTime => ApplicationStatisticsModel.LastHttpResponseDateTime;
    public long LastUdpReadingWaitMilliseconds => ApplicationStatisticsModel.LastUdpReadingWaitMilliseconds;
    public long LastHttpResponseWaitMilliseconds => ApplicationStatisticsModel.LastHttpResponseWaitMilliseconds;
    public long UdpWaitTimeTotalMilliseconds => ApplicationStatisticsModel.UdpWaitTimeTotalMilliseconds;
    public long HttpResponseWaitTimeTotalMilliseconds => ApplicationStatisticsModel.HttpResponseWaitTimeTotalMilliseconds;
    public string AreUdpBroadcastsBeingListenedFor => ApplicationStatisticsModel.AreUdpBroadcastsBeingListenedFor ? "Yes" : "No";
    public string AreHttpRequestsBeingMade => ApplicationStatisticsModel.AreHttpRequestsBeingMade ? "Yes" : "No";
    public double? AverageUdpReadingWaitMilliseconds
    {
        get
        {
            if (ApplicationStatisticsModel.UdpReadingCount == 0) return null;
            return ApplicationStatisticsModel.UdpWaitTimeTotalMilliseconds / ApplicationStatisticsModel.UdpReadingCount;
        }
    }
    public double? AverageHttpResponseWaitMilliseconds
    {
        get
        {
            if (ApplicationStatisticsModel.HttpResponseCount == 0) return null;
            return ApplicationStatisticsModel.HttpResponseWaitTimeTotalMilliseconds / ApplicationStatisticsModel.HttpResponseCount;
        }
    }
    public long? SecondsSinceLastUdpReading => (LastUdpReadingDateTime is null ? null : (DateTime.Now - LastUdpReadingDateTime).Value.Seconds);
    public long? MinutesSinceLastHttpResponse => LastHttpResponseDateTime is null ? null : (DateTime.Now - LastHttpResponseDateTime).Value.Minutes;
    public long? TimeBetweenHttpRequestsInMinutes => _settings.TimeBetweenHttpRequestsInMinutes;
    private void UpdateStatistics(object? stateInfo)
    {
        OnPropertyChanged(nameof(DeviceStatusReceivedCount));
        OnPropertyChanged(nameof(LastDeviceStatusReceivedDateTime));

        OnPropertyChanged(nameof(ForecastReceivedCount));
        OnPropertyChanged(nameof(LastForecastReceivedDateTime));

        OnPropertyChanged(nameof(HubStatusReceivedCount));
        OnPropertyChanged(nameof(LastHubStatusReceivedDateTime));

        OnPropertyChanged(nameof(LightningStrikeReceivedCount));
        OnPropertyChanged(nameof(LastLightningStrikeReceivedDateTime));

        OnPropertyChanged(nameof(ObservationReadingReceivedCount));
        OnPropertyChanged(nameof(LastObservationReadingReceivedDateTime));

        OnPropertyChanged(nameof(RainStartReceivedCount));
        OnPropertyChanged(nameof(LastRainStartReceivedDateTime));

        OnPropertyChanged(nameof(WindReadingReceivedCount));
        OnPropertyChanged(nameof(LastWindReadingReceivedDateTime));
        
        OnPropertyChanged(nameof(CurrentTime));

        OnPropertyChanged(nameof(AreUdpBroadcastsBeingListenedFor));
        OnPropertyChanged(nameof(SecondsSinceLastUdpReading));
        OnPropertyChanged(nameof(LastUdpReadingDateTime));
        OnPropertyChanged(nameof(LastUdpReadingWaitMilliseconds));
        OnPropertyChanged(nameof(UdpReadingCount));
        OnPropertyChanged(nameof(UdpWaitTimeTotalMilliseconds));
        OnPropertyChanged(nameof(AverageUdpReadingWaitMilliseconds));

        OnPropertyChanged(nameof(AreHttpRequestsBeingMade));
        OnPropertyChanged(nameof(MinutesSinceLastHttpResponse));
        OnPropertyChanged(nameof(LastHttpResponseDateTime));
        OnPropertyChanged(nameof(LastHttpResponseWaitMilliseconds));
        OnPropertyChanged(nameof(HttpResponseCount));
        OnPropertyChanged(nameof(HttpResponseWaitTimeTotalMilliseconds));
        OnPropertyChanged(nameof(AverageHttpResponseWaitMilliseconds));

        OnPropertyChanged(nameof(TimeBetweenHttpRequestsInMinutes));
    }

}
