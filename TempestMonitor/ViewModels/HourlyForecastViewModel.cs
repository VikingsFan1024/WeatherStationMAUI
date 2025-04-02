using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using CommunityToolkit.Mvvm.Messaging;

using TempestMonitor.Models;
using TempestMonitor.Services;
using TempestMonitor.ViewModels.Observables;

namespace TempestMonitor.ViewModels;

sealed partial class HourlyForecastViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private ObservableCollection<HourlyObservable>? _hourlyForecasts;
    public ObservableCollection<HourlyObservable>? HourlyForecasts => _hourlyForecasts;
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
                _hourlyForecasts = HourlyObservable.ConvertToObservableCollection(
                    m.Forecast.Hourlies, _settings);
                OnPropertyChanged(nameof(HourlyForecasts));
            }
        );

        var hourlies = serviceProvider.GetRequiredService<RequestForecastsService>()
                .MostRecentForecast?.Hourlies;

        if (hourlies is not null)
            _hourlyForecasts = HourlyObservable.ConvertToObservableCollection(hourlies, _settings);

        OnPropertyChanged(nameof(HourlyForecasts));
    }
}   
