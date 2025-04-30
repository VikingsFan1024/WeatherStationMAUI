namespace TempestMonitor.ViewModels;

sealed partial class ForecastViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    ObservableWeatherForecastGraph? _observableForecast;

    public void OnDisappearing()
    {
        _foregroundServiceHandler.Unregister(this);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void OnAppearing()
    {
        _foregroundServiceHandler.Register(this);
        WeakReferenceMessenger.Default.Register<VW_Message<WeatherForecastGraph>>
        (
            this, (r, m) =>
            {
                _observableForecast = new(m.Model, _settings);
                OnPropertyChanged(nameof(ObservableForecast));
            }
        );

        var temporaryForecast = serviceProvider.GetRequiredService<ReadingBroadcastService>().MostRecentVW_WeatherForecastModel; ;
        if (temporaryForecast is not null)
        {
            _observableForecast = new(temporaryForecast, _settings);
            OnPropertyChanged(nameof(ObservableForecast));
        }
    }
    public ObservableWeatherForecastGraph? ObservableForecast => _observableForecast;
}