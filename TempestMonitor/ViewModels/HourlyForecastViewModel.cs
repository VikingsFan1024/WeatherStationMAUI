using static CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;  // for Register method
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ObservableCollectionOfObservableHourly = System.Collections.ObjectModel.ObservableCollection<TempestMonitor.ViewModels.Observables.ObservableHourly>;

using CallerMemberNameAttribute = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using ForegroundServiceHandler = TempestMonitor.Services.ForegroundServiceHandler; // For foreground service handling
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;
using IServiceProvider = System.IServiceProvider;
using ObservableHourly = TempestMonitor.ViewModels.Observables.ObservableHourly;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
using PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;
using RequestForecastsService = TempestMonitor.Services.RequestForecastsService; // For RequestForecastsService
using SettingsModel = TempestMonitor.Models.SettingsModel;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

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
