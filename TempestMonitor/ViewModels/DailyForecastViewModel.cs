namespace TempestMonitor.ViewModels;

sealed partial class DailyForecastViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private ObservableCollectionOfObservableDaily? _dailyForecasts;
    public ObservableCollectionOfObservableDaily? DailyForecasts => _dailyForecasts;

    public void OnDisappearing()
    {
        // Unregister call will stop services if only user and think that's always true
        _foregroundServiceHandler.Unregister(this);

        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public void OnAppearing()
    {
        WeakReferenceMessenger.Default.Register<VW_Message<WeatherForecastGraph>>
        (
            this, (r, m) =>
            {
                _dailyForecasts = ObservableDaily.ConvertToObservableCollection(
                    m.Model.GetTempestRedStarMapping(), m.Model.forecast.daily, _settings);
                OnPropertyChanged(nameof(DailyForecasts));
            }
        );

        // Register call will start services if only user and think that's always true
        _foregroundServiceHandler.Register(this);
    }
}
