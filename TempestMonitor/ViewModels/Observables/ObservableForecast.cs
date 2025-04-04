using ObservableCollectionOfObservableDaily = System.Collections.ObjectModel.ObservableCollection<TempestMonitor.ViewModels.Observables.ObservableDaily>;
using ObservableCollectionOfObservableHourly = System.Collections.ObjectModel.ObservableCollection<TempestMonitor.ViewModels.Observables.ObservableHourly>;

using DateTime = System.DateTime;
using ForecastModel = TempestMonitor.Models.ForecastModel;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;
using SettingsModel = TempestMonitor.Models.SettingsModel;

namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableForecast : ObservableObject
{
    private readonly ForecastModel _forecast;
    private readonly ObservableCurrentConditions _currentConditions;
    private readonly ObservableCollectionOfObservableDaily _observableDailies;
    private readonly ObservableCollectionOfObservableHourly _observableHourlies;
    private readonly ObservableStation _stationObservable;
    private readonly ObservableUnits _unitsObservable;
    public ObservableForecast(ForecastModel forecast, SettingsModel settings) : base()
    {
        _forecast = forecast;
        _currentConditions = new ObservableCurrentConditions(forecast.CurrentConditions, settings);
        _observableHourlies = ObservableHourly.ConvertToObservableCollection(forecast.Hourlies, settings);
        _observableDailies = ObservableDaily.ConvertToObservableCollection(forecast.Dailies, settings);
        _stationObservable = new ObservableStation(forecast.Station, settings);
        _unitsObservable = new ObservableUnits(forecast.Units);
    }

    public DateTime Timestamp => Constants.UnixSecondsToLocalTime(_forecast.Timestamp);
    public double Latitude => Constants.LongToDouble(_forecast.Latitude);
    public string LocationName => _forecast.LocationName;
    public double Longitude => Constants.LongToDouble(_forecast.Longitude);
    public string Timezone => _forecast.Timezone;
    public long TimezoneOffsetMinutes => _forecast.TimezoneOffsetMinutes;
    public long SourceIdConditions => _forecast.SourceIdConditions;
    public ObservableCurrentConditions CurrentConditions => _currentConditions;
    public ObservableCollectionOfObservableDaily DailyForecasts => _observableDailies;
    public ObservableCollectionOfObservableHourly HourlyForecasts => _observableHourlies;
    public ObservableStation Station => _stationObservable;
    public ObservableUnits Units => _unitsObservable;
}
