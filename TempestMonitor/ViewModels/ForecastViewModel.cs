using static CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;  // for Register method
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using CallerMemberNameAttribute = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using ForegroundServiceHandler = TempestMonitor.Services.ForegroundServiceHandler; // For foreground service handling
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;
using IServiceProvider = System.IServiceProvider;
using ObservableForecast = TempestMonitor.ViewModels.Observables.ObservableForecast; // For ObservableForecast
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
using PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;
using RequestForecastsService = TempestMonitor.Services.RequestForecastsService; // For RequestForecastsService
using SettingsModel = TempestMonitor.Models.SettingsModel;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace TempestMonitor.ViewModels;

sealed partial class ForecastViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    ObservableForecast? _observableForecast;

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
    public ObservableForecast? ObservableForecast => _observableForecast;
}