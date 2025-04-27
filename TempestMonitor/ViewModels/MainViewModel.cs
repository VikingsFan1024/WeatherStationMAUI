using TempestMonitor.ViewModels.Observables;

namespace TempestMonitor.ViewModels;

sealed partial class MainViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private ObservableVW_WindModel? _observableVW_WindModel;
    private ObservableVW_ObservationModel? _observableVW_Observation;

    private Timer? _timer;
    private DateTime _currentDateTime = DateTime.Now;
    public string TemperatureUnitSymbol => _settings.TemperatureUnitSymbol;
    public string WindspeedUnitSymbol => _settings.WindspeedUnitSymbol;
    public void Dispose()
    {
        _timer?.Dispose();
        _timer = null;

        GC.SuppressFinalize(this);
    }
    public void OnAppearing()
    {
        _foregroundServiceHandler?.Register(this);

        _timer?.Dispose();
        _timer = null;
        _timer = new Timer(new TimerCallback(UpdateReadings), null, TimeSpan.FromMilliseconds(1), TimeSpan.FromSeconds(1));

        WeakReferenceMessenger.Default.Register<VW_Message<VW_WindModel>>
        (
            this, (r, m) =>
            {
                _observableVW_WindModel = new(m.Model, _settings);
                OnPropertyChanged(nameof(ObservableVW_WindModel));
            }
        );

        WeakReferenceMessenger.Default.Register<VW_Message<VW_ObservationModel>>
        (
            this, (r, m) =>
            {
                _observableVW_Observation = new(m.Model, _settings);
                OnPropertyChanged(nameof(ObservableVW_Observation));
                OnPropertyChanged(nameof(TemperatureUnitSymbol));
            }
        );

        var vw_WindModel = serviceProvider.GetRequiredService<ReadingBroadcastService>()
            .MostRecentVW_WindModel;
        if (vw_WindModel is not null)
        {
            _observableVW_WindModel = new(vw_WindModel, _settings);
            OnPropertyChanged(nameof(ObservableVW_WindModel));
            OnPropertyChanged(nameof(WindspeedUnitSymbol));
        }

        var vw_ObservationModel = serviceProvider.GetRequiredService<ReadingBroadcastService>()
            .MostRecentVW_ObservationModel;
        if (vw_ObservationModel != null)
        {
            _observableVW_Observation = new(vw_ObservationModel, _settings);
            OnPropertyChanged(nameof(ObservableVW_Observation));
            OnPropertyChanged(nameof(TemperatureUnitSymbol));
        }
    }
    public void OnDisappearing()
    {
        _foregroundServiceHandler?.Unregister(this);
        WeakReferenceMessenger.Default.UnregisterAll(this);
        _timer?.Dispose();
        _timer = null;
    }
#pragma warning disable CA1822 // Can't be static for it to bind with UI elements
    public DateTime CurrentDateTime => _currentDateTime;
    public string CurrentDateTimeHour => _currentDateTime.ToString(_settings.TimeFormat, CultureInfo.InvariantCulture);
#pragma warning restore CA822
    public long? MinutesSinceLastHttpResponse =>
        ApplicationStatisticsModel.LastHttpResponseDateTime is null ? null :
            (DateTime.Now - ApplicationStatisticsModel.LastHttpResponseDateTime).Value.Minutes;
    public ObservableVW_ObservationModel? ObservableVW_Observation => _observableVW_Observation;
    public long? SecondsSinceLastUdpReading =>
        ApplicationStatisticsModel.LastUdpReadingDateTime is null ? null :
            (DateTime.Now - ApplicationStatisticsModel.LastUdpReadingDateTime).Value.Seconds;
    public ObservableVW_WindModel? ObservableVW_WindModel => _observableVW_WindModel;
    private void UpdateReadings(object? stateInfo)
    {
        _currentDateTime = DateTime.Now;
        OnPropertyChanged(nameof(CurrentDateTime));
        OnPropertyChanged(nameof(CurrentDateTimeHour));
    }
}