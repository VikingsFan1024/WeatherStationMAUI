using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;

using TempestMonitor.Models;
using TempestMonitor.ViewModels.Observables;
using TempestMonitor.Services;
using RedStar.Amounts;
using RedStar.Amounts.StandardUnits;

namespace TempestMonitor.ViewModels;

sealed partial class LiveStationReadingsViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();

    private ObservableObservation? _observableObservation;
    private ObservableWindReading? _observableWindReading;

    public void OnDisappearing()
    {
        _foregroundServiceHandler.Unregister(this);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void OnAppearing()
    {
        _foregroundServiceHandler.Register(this);
        WeakReferenceMessenger weakReferenceMessenger = WeakReferenceMessenger.Default;
        weakReferenceMessenger.Register<ReadingsListenerService.WindReadingMessage>
        (
            this, (r, m) =>
            {
                _observableWindReading = new(m.WindReading, _settings);
                OnPropertyChanged(nameof(ObservableWindReading));
                OnPropertyChanged(nameof(CalculatedFeelsLike));
                OnPropertyChanged(nameof(CalculatedWindChill));
            }
        );
        weakReferenceMessenger.Register<ReadingsListenerService.ObservationReadingMessage>
        (
            this, (r, m) =>
            {
                _observableObservation = new(m.ObservationReading, _settings);
                OnPropertyChanged(nameof(ObservableObservation));
                OnPropertyChanged(nameof(CalculatedFeelsLike));
                OnPropertyChanged(nameof(CalculatedHeatIndex));
                OnPropertyChanged(nameof(CalculatedWindChill));
            }
        );

        var observation = serviceProvider.GetRequiredService<ReadingsListenerService>().MostRecentObservationReading;
        if (observation is not null) _observableObservation = new(observation, _settings);
        OnPropertyChanged(nameof(ObservableObservation));

        var windReading = serviceProvider.GetRequiredService<ReadingsListenerService>().MostRecentWindReading;
        if (windReading is not null) _observableWindReading = new(windReading, _settings);
        OnPropertyChanged(nameof(ObservableWindReading));

        OnPropertyChanged(nameof(CalculatedFeelsLike));
        OnPropertyChanged(nameof(CalculatedHeatIndex));
        OnPropertyChanged(nameof(CalculatedWindChill));
    }
    public double? CalculatedFeelsLike
    {
        get
        {
            if (_observableObservation is null || _observableWindReading is null) return null;

            var windspeedInMPH = new Amount(
                    Constants.LongToDouble(_observableWindReading.WindReading.Windspeed),
                    WindReadingModel.PropertyUnit[nameof(WindReadingModel.Windspeed)]
                ).ConvertedTo(UnitManager.GetUnitByName(SpeedUnits.MilePerHour.Name)).Value;

            var temperatureInFahrenheit = new Amount(
                    Constants.LongToDouble(_observableObservation.Observation.AirTemperature),
                    ObservationModel.PropertyUnit[nameof(ObservationModel.AirTemperature)]
                ).ConvertedTo(UnitManager.GetUnitByName(TemperatureUnits.DegreeFahrenheit.Name)).Value;

            var feelsLikeInFahrenheit = WeatherUtilities.CalculateFeelsLike(
                temperatureInFahrenheit,
                _observableObservation.RelativeHumidity,
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
            if (_observableObservation is null) return null;

            var temperatureInFahrenheit = new Amount(
                    Constants.LongToDouble(_observableObservation.Observation.AirTemperature),
                    ObservationModel.PropertyUnit[nameof(ObservationModel.AirTemperature)]
                ).ConvertedTo(UnitManager.GetUnitByName(TemperatureUnits.DegreeFahrenheit.Name)).Value;

            var heatIndexInFahrenheit = WeatherUtilities.CalculateHeatIndex(
                temperatureInFahrenheit,
                _observableObservation.RelativeHumidity
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
            if (_observableObservation is null || _observableWindReading is null) return null;

            var temperatureInFahrenheit = new Amount(
                    Constants.LongToDouble(_observableObservation.Observation.AirTemperature),
                    ObservationModel.PropertyUnit[nameof(ObservationModel.AirTemperature)]
                ).ConvertedTo(UnitManager.GetUnitByName(TemperatureUnits.DegreeFahrenheit.Name)).Value;

            var windspeedInMPH = new Amount(
                    Constants.LongToDouble(_observableWindReading.WindReading.Windspeed),
                    WindReadingModel.PropertyUnit[nameof(WindReadingModel.Windspeed)]
                ).ConvertedTo(UnitManager.GetUnitByName(SpeedUnits.MilePerHour.Name)).Value;

            var feelsLikeInFahrenheit = WeatherUtilities.CalculateWindChill(
                temperatureInFahrenheit,windspeedInMPH);

            return new Amount(feelsLikeInFahrenheit,TemperatureUnits.DegreeFahrenheit)
                .ConvertedTo(_settings.TemperatureUnit).Value;
        }
    }
    public ObservableObservation? ObservableObservation => _observableObservation;
    public ObservableWindReading? ObservableWindReading => _observableWindReading;
    public SettingsModel Settings => _settings;
}
