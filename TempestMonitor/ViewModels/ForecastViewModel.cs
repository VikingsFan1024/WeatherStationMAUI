using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;

using TempestMonitor.Models;
using TempestMonitor.Services;
using TempestMonitor.ViewModels.Observables;

namespace TempestMonitor.ViewModels;

sealed partial class ForecastViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    ForecastObservable? _observableForecast;

    public void OnDisappearing()
    {
        _foregroundServiceHandler.Unregister(this);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void OnAppearing()
    {
        _foregroundServiceHandler.Register(this);
        WeakReferenceMessenger.Default.Register<RequestForecastsService.ForecastMessage>(this, (r, m) =>
        {
            _observableForecast = new(m.Forecast, _settings);
            OnPropertyChanged(nameof(ObservableForecast));
        });

        var temporaryForecast = serviceProvider.GetRequiredService<RequestForecastsService>().MostRecentForecast;
        if (temporaryForecast is not null)
        {
            _observableForecast = new(temporaryForecast, _settings);
            OnPropertyChanged(nameof(ObservableForecast));
        }
    }
    public ForecastObservable? ObservableForecast => _observableForecast;
}