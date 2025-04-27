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
        WeakReferenceMessenger.Default.Register<VW_Message<WeatherForecastGraph>>
        (
            this, (r, m) =>
            {
                _hourlyForecasts = ObservableHourly.ConvertToObservableCollection(
                    m.Model.GetTempestRedStarMapping(), m.Model.forecast.hourly, _settings);
                OnPropertyChanged(nameof(HourlyForecasts));
            }
        );

        //var vw_WeatherForecastModel = serviceProvider.GetRequiredService<ReadingBroadcastService>()
        //        .MostRecentVW_WeatherForecastModel;

        //if (vw_WeatherForecastModel is not null)
        //{
        //    var hourlies = vw_WeatherForecastModel.forecast.hourly;
        //    if (hourlies is not null)
        //        _hourlyForecasts = ObservableHourly.ConvertToObservableCollection(
        //            vw_WeatherForecastModel.GetTempestRedStarMapping(), hourlies, _settings);
        //}

        //OnPropertyChanged(nameof(HourlyForecasts));
    }
}
