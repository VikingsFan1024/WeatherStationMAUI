namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableForecast : ObservableObject
{
    private readonly WeatherForecastGraph _forecast;
    private readonly ObservableCurrentConditions _currentConditions;
    private readonly ObservableCollectionOfObservableHourly _observableHourlies;
    private readonly ObservableCollectionOfObservableDaily _observableDailies;
    private readonly ObservableStation _stationObservable;
    private readonly ObservableUnits _unitsObservable;
    public ObservableForecast(WeatherForecastGraph weatherForecast, SettingsModel settings) : base()
    {
        _forecast = weatherForecast;
        var _tempestRedStarMapping = weatherForecast.GetTempestRedStarMapping();
        _currentConditions = new ObservableCurrentConditions(
            _tempestRedStarMapping, weatherForecast.current_conditions, settings);
        _observableHourlies = ObservableHourly.ConvertToObservableCollection(
            _tempestRedStarMapping, weatherForecast.forecast.hourly, settings);
        _observableDailies = ObservableDaily.ConvertToObservableCollection(
            _tempestRedStarMapping, weatherForecast.forecast.daily, settings);
        _stationObservable = new ObservableStation(
            _tempestRedStarMapping, weatherForecast.station, settings);
        _unitsObservable = new ObservableUnits(weatherForecast.units);
    }

    public double latitude => _forecast.latitude;
    public string location_name => _forecast.location_name;
    public double longitude => _forecast.longitude;
    public string timezone => _forecast.timezone;
    public long timezone_offset_minutes => _forecast.timezone_offset_minutes;
    public long source_id_conditions => _forecast.source_id_conditions;
    public ObservableCurrentConditions CurrentConditions => _currentConditions;
    public ObservableCollectionOfObservableDaily DailyForecasts => _observableDailies;
    public ObservableCollectionOfObservableHourly HourlyForecasts => _observableHourlies;
    public ObservableStation Station => _stationObservable;
    public ObservableUnits Units => _unitsObservable;
}
