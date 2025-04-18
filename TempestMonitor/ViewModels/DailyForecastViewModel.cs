﻿namespace TempestMonitor.ViewModels;

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
                _dailyForecasts = ObservableDaily.ConvertToObservableCollection(
                    m.Forecast.Dailies, _settings);
                OnPropertyChanged(nameof(DailyForecasts));
            }
        );

        var dailies = serviceProvider.GetRequiredService<RequestForecastsService>()
            .MostRecentForecast?.Dailies;

        if (dailies is not null)
            _dailyForecasts = ObservableDaily.ConvertToObservableCollection(dailies, _settings);

        OnPropertyChanged(nameof(DailyForecasts));
    }
}
