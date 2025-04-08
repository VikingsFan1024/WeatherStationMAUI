namespace TempestMonitor.ViewModels;

sealed partial class HourlyForecastViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private ObservableCollectionOfObservableHourly? _hourlyForecasts;
    public ObservableCollectionOfObservableHourly? HourlyForecasts => _hourlyForecasts;
    public void OnDisappearing()
    {
        _foregroundServiceHandler.Unregister(this);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public void OnAppearing()
    {
        _foregroundServiceHandler.Register(this);
        WeakReferenceMessenger.Default.Register<RequestForecastsService.ForecastMessage>
        (
            this, (r, m) =>
            {
                _hourlyForecasts = ObservableHourly.ConvertToObservableCollection(
                    m.Forecast.Hourlies, _settings);
                OnPropertyChanged(nameof(HourlyForecasts));
            }
        );

        var hourlies = serviceProvider.GetRequiredService<RequestForecastsService>()
                .MostRecentForecast?.Hourlies;

        if (hourlies is not null)
            _hourlyForecasts = ObservableHourly.ConvertToObservableCollection(hourlies, _settings);

        OnPropertyChanged(nameof(HourlyForecasts));
    }
}   
