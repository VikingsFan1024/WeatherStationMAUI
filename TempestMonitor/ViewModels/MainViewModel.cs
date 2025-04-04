using static CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;  // for Register method
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ApplicationStatisticsModel = TempestMonitor.Models.ApplicationStatisticsModel; // For accessing the static model
using DateTime = System.DateTime;
using ForegroundServiceHandler = TempestMonitor.Services.ForegroundServiceHandler; // For foreground service handling
using IDisposable = System.IDisposable;
using IServiceProvider = System.IServiceProvider;
using TimeSpan = System.TimeSpan;
using CallerMemberNameAttribute = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using CultureInfo = System.Globalization.CultureInfo;
using GC = System.GC;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;
using ObservableObservation = TempestMonitor.ViewModels.Observables.ObservableObservation;
using ObservableWindReading = TempestMonitor.ViewModels.Observables.ObservableWindReading;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
using PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;
using ReadingsListenerService = TempestMonitor.Services.ReadingsListenerService;
using SettingsModel = TempestMonitor.Models.SettingsModel;
using Timer = System.Threading.Timer; // Avoid conflict with System.Timers.Timer
using TimerCallback = System.Threading.TimerCallback;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace TempestMonitor.ViewModels;

sealed partial class MainViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private ObservableWindReading? _observableWindReading;
    private ObservableObservation? _observableObservation;

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

        WeakReferenceMessenger.Default.Register<ReadingsListenerService.WindReadingMessage>
        (
            this, (r, m) =>
            {
                _observableWindReading = new(m.WindReading, _settings);
                OnPropertyChanged(nameof(WindReading));
            }
        );

        WeakReferenceMessenger.Default.Register<ReadingsListenerService.ObservationReadingMessage>
        (
            this, (r, m) =>
            {
                _observableObservation = new(m.ObservationReading, _settings);
                OnPropertyChanged(nameof(ObservationReading));
                OnPropertyChanged(nameof(TemperatureUnitSymbol));
            }
        );

        var windReadingModel = serviceProvider.GetRequiredService<ReadingsListenerService>()
            .MostRecentWindReading;
        if (windReadingModel != null)
        {
            _observableWindReading = new(windReadingModel, _settings);
            OnPropertyChanged(nameof(WindReading));
            OnPropertyChanged(nameof(WindspeedUnitSymbol));
        }

        var observationReadingModel = serviceProvider.GetRequiredService<ReadingsListenerService>()
            .MostRecentObservationReading;
        if (observationReadingModel != null)
        {
            _observableObservation = new(observationReadingModel, _settings);
            OnPropertyChanged(nameof(ObservationReading));
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
    public ObservableObservation? ObservationReading => _observableObservation;
    public long? SecondsSinceLastUdpReading =>
        ApplicationStatisticsModel.LastUdpReadingDateTime is null ? null :
            (DateTime.Now - ApplicationStatisticsModel.LastUdpReadingDateTime).Value.Seconds;
    public ObservableWindReading? WindReading => _observableWindReading;
    private void UpdateReadings(object? stateInfo)
    {
        _currentDateTime = DateTime.Now;
        OnPropertyChanged(nameof(CurrentDateTime));
        OnPropertyChanged(nameof(CurrentDateTimeHour));
    }
}