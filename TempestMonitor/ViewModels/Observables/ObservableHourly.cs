namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableHourly : ObervableBase
{
    private readonly Hourly _hourly;

    public ObservableHourly(
        TempestRedStarMapping tempestRedStarMapping, Hourly hourly, int rowNumber, SettingsModel settings) : base(settings)
    {
        _hourly = hourly;
        RowNumber = rowNumber;

        // TODO: Change so ConvertedTo call uses Unit and doesn't have to lookup using name
        air_temperature = new Amount(_hourly.air_temperature, tempestRedStarMapping.units_temp)
            .ConvertedTo(_settings.TemperatureUnit).Value;

        feels_like = new Amount(_hourly.feels_like, tempestRedStarMapping.units_temp)
            .ConvertedTo(_settings.TemperatureUnit).Value;

        precip = new Amount(_hourly.precip, tempestRedStarMapping.units_precip)
            .ConvertedTo(_settings.PrecipitationUnit).Value;

        sea_level_pressure = new Amount(_hourly.sea_level_pressure, tempestRedStarMapping.units_pressure)
            .ConvertedTo(_settings.PressureUnit).Value;

        station_pressure = new Amount(_hourly.station_pressure, tempestRedStarMapping.units_pressure)
            .ConvertedTo(_settings.PressureUnit).Value;

        wind_avg = new Amount(_hourly.wind_avg, tempestRedStarMapping.units_wind)
            .ConvertedTo(_settings.WindspeedUnit).Value;
    }
    public int RowNumber { get; private set; }
    public double air_temperature { get; private set; }
    public string conditions => _hourly.conditions;
    public double feels_like { get; private set; }
    public DateTime time => Constants.UnixSecondsToDateTime(_hourly.time);
    public double precip { get; private set; }
    public double precip_probability => _hourly.precip_probability;
    public double relative_humidity => _hourly.relative_humidity;
    public double sea_level_pressure { get; private set; }
    public double station_pressure { get; private set; }
    public double uv => _hourly.uv;
    public double wind_avg { get; private set; }
    public double wind_direction => _hourly.wind_direction;
    public long local_day => _hourly.local_day;
    public long local_hour => _hourly.local_hour;
    public string icon => _hourly.icon;
    public string precip_icon => _hourly.precip_icon;
    public string precip_type => _hourly.precip_type;
    public string wind_direction_cardinal => _hourly.wind_direction_cardinal;
    public static ObservableCollectionOfObservableHourly ConvertToObservableCollection(
        TempestRedStarMapping tempestRedStarMapping, Hourly[] hourlies, SettingsModel settings, int skipCount = 0, int takeCount = 40)
    {
        int rowNumber = 1;

        // Creating these is fast but displaying them is not
        var result = new ObservableCollectionOfObservableHourly(
            hourlies.Select(hourly => new ObservableHourly(tempestRedStarMapping, hourly, rowNumber++, settings))
            .Skip(skipCount).Take(takeCount).ToList()
        );

        return result;
    }
}