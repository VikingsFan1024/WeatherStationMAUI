namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableWeatherForecastGraph : ObservableObject
{
    private readonly WeatherForecastGraph _weatherForecastGraph;
    private readonly ObservableCurrentConditions _currentConditions;
    private readonly ObservableCollectionOfObservableHourly _observableHourlies;
    private readonly ObservableCollectionOfObservableDaily _observableDailies;
    private readonly ObservableStation _stationObservable;
    private readonly ObservableUnits _unitsObservable;
    public ObservableWeatherForecastGraph(WeatherForecastGraph weatherForecast, SettingsModel settings) : base()
    {
        _weatherForecastGraph = weatherForecast;
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

    public double latitude => _weatherForecastGraph.latitude;
    public string location_name => _weatherForecastGraph.location_name;
    public double longitude => _weatherForecastGraph.longitude;
    public string timezone => _weatherForecastGraph.timezone;
    public long timezone_offset_minutes => _weatherForecastGraph.timezone_offset_minutes;
    public long source_id_conditions => _weatherForecastGraph.source_id_conditions;
    public ObservableCurrentConditions CurrentConditions => _currentConditions;
    public ObservableCollectionOfObservableDaily DailyForecasts => _observableDailies;
    public ObservableCollectionOfObservableHourly HourlyForecasts => _observableHourlies;
    public ObservableStation Station => _stationObservable;
    public ObservableUnits Units => _unitsObservable;
}
