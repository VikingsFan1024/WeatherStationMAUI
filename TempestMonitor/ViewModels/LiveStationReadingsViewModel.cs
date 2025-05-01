using TempestMonitor.ViewModels.Observables;

namespace TempestMonitor.ViewModels;

public sealed partial class LiveStationReadingsViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private ObservableVW_ObservationModel? _observableVW_ObservationModel;
    private ObservableVW_WindModel? _observableVW_WindModel;

    public ObservableVW_ObservationModel? ObservableVW_ObservationModel => _observableVW_ObservationModel;
    public ObservableVW_WindModel? ObservableVW_WindModel => _observableVW_WindModel;
    public SettingsModel Settings => _settings;

    public void OnDisappearing()
    {
        _foregroundServiceHandler.Unregister(this);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void OnAppearing()
    {
        WeakReferenceMessenger weakReferenceMessenger = WeakReferenceMessenger.Default;
        weakReferenceMessenger.Register<VW_Message<VW_WindModel>>
        (
            this, (r, m) =>
            {
                _observableVW_WindModel = new(m.Model, _settings);
                OnPropertyChanged(nameof(ObservableVW_WindModel));
                OnPropertyChanged(nameof(CalculatedFeelsLike));
                OnPropertyChanged(nameof(CalculatedWindChill));
            }
        );
        weakReferenceMessenger.Register<VW_Message<VW_ObservationModel>>
        (
            this, (r, m) =>
            {
                _observableVW_ObservationModel = new(m.Model, _settings);
                OnPropertyChanged(nameof(ObservableVW_ObservationModel));
                OnPropertyChanged(nameof(CalculatedFeelsLike));
                OnPropertyChanged(nameof(CalculatedHeatIndex));
                OnPropertyChanged(nameof(CalculatedWindChill));
            }
        );

        _foregroundServiceHandler.Register(this);

        // At time of comment if no users/registrations with _foregroundServiceHandler then Appearing causes start and Disappearing causes stop
        //var observation = serviceProvider.GetRequiredService<ReadingsListenerService>().MostRecentObservationReading;
        //if (observation is not null) _observableObservation = new(observation, _settings);
        //OnPropertyChanged(nameof(ObservableObservation));

        //var windReading = serviceProvider.GetRequiredService<ReadingBroadcastService>().MostRecentVW_WindModel;
        //if (windReading is not null) _observableWindReading = new(windReading, _settings);
        //OnPropertyChanged(nameof(ObservableWindReading));

        //OnPropertyChanged(nameof(CalculatedFeelsLike));
        //OnPropertyChanged(nameof(CalculatedHeatIndex));
        //OnPropertyChanged(nameof(CalculatedWindChill));
    }
    public double? CalculatedFeelsLike
    {
        get
        {
            if (_observableVW_ObservationModel is null || _observableVW_WindModel is null) return null;

            // ToDo: CHange so .ConvertedTo() is using a parameter of Unit not string so it doesn't have to look up the unit by name
            var windspeedInMPH = new Amount(
                    _observableVW_WindModel.wind_speed,
                    _settings.WindspeedUnit
                ).ConvertedTo(UnitManager.GetUnitByName(SpeedUnits.MilePerHour.Name)).Value;

            var temperatureInFahrenheit = new Amount(
                    _observableVW_ObservationModel.air_temperature,
                    _settings.TemperatureUnit
                ).ConvertedTo(UnitManager.GetUnitByName(TemperatureUnits.DegreeFahrenheit.Name)).Value;

            var feelsLikeInFahrenheit = WeatherUtilities.CalculateFeelsLike(
                temperatureInFahrenheit,
                _observableVW_ObservationModel.relative_humidity,
                windspeedInMPH
            );

            return new Amount(feelsLikeInFahrenheit, TemperatureUnits.DegreeFahrenheit)
                .ConvertedTo(_settings.TemperatureUnit).Value;
        }
    }

    public double? CalculatedHeatIndex
    {
        get
        {
            if (_observableVW_ObservationModel is null) return null;

            var temperatureInFahrenheit = new Amount(
                    _observableVW_ObservationModel.air_temperature,
                    _settings.TemperatureUnit
                ).ConvertedTo(UnitManager.GetUnitByName(TemperatureUnits.DegreeFahrenheit.Name)).Value;

            var heatIndexInFahrenheit = WeatherUtilities.CalculateHeatIndex(
                temperatureInFahrenheit,
                _observableVW_ObservationModel.relative_humidity
            );

            return new Amount(
                heatIndexInFahrenheit,
                TemperatureUnits.DegreeFahrenheit
            ).ConvertedTo(_settings.TemperatureUnit).Value;
        }
    }
    public double? CalculatedWindChill
    {
        get
        {
            if (_observableVW_ObservationModel is null || _observableVW_WindModel is null) return null;

            var temperatureInFahrenheit = new Amount(
                    _observableVW_ObservationModel.air_temperature,
                    _settings.TemperatureUnit
                ).ConvertedTo(UnitManager.GetUnitByName(TemperatureUnits.DegreeFahrenheit.Name)).Value;

            var windspeedInMPH = new Amount(
                    _observableVW_WindModel.wind_speed,
                    _settings.WindspeedUnit
                ).ConvertedTo(UnitManager.GetUnitByName(SpeedUnits.MilePerHour.Name)).Value;

            var feelsLikeInFahrenheit = WeatherUtilities.CalculateWindChill(
                temperatureInFahrenheit, windspeedInMPH);

            return new Amount(feelsLikeInFahrenheit, TemperatureUnits.DegreeFahrenheit)
                .ConvertedTo(_settings.TemperatureUnit).Value;
        }
    }
}
