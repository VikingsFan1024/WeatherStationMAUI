namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableCurrentConditions : ObervableBase
{
    private readonly CurrentConditions _currentConditions;
    public ObservableCurrentConditions(
        TempestRedStarMapping tempestRedStarMapping,
        CurrentConditions currentConditions,
        SettingsModel settings) : base(settings)
    {
        _currentConditions = currentConditions;

        // ToDo: All Converted two should use the Unit vs having it lookup the unit
        // The lookup should be done once for the settings similar to not so great tempestRedStarMapping
        air_temperature = new Amount(_currentConditions.air_temperature, tempestRedStarMapping.units_temp)
            .ConvertedTo(_settings.TemperatureUnit).Value;

        dew_point = new Amount(_currentConditions.dew_point, tempestRedStarMapping.units_temp)
            .ConvertedTo(_settings.TemperatureUnit).Value;

        feels_like = new Amount(_currentConditions.feels_like, tempestRedStarMapping.units_temp)
            .ConvertedTo(_settings.TemperatureUnit).Value;

        precip_accum_local_day = new Amount(
            _currentConditions.precip_accum_local_day, tempestRedStarMapping.units_precip)
            .ConvertedTo(_settings.PrecipitationUnit).Value;

        precip_accum_local_yesterday = new Amount(
            _currentConditions.precip_accum_local_yesterday, tempestRedStarMapping.units_precip).
            ConvertedTo(_settings.PrecipitationUnit).Value;

        sea_level_pressure = new Amount(_currentConditions.sea_level_pressure, tempestRedStarMapping.units_pressure)
            .ConvertedTo(_settings.PressureUnit).Value;

        station_pressure = new Amount(_currentConditions.station_pressure, tempestRedStarMapping.units_pressure).
            ConvertedTo(_settings.PressureUnit).Value;

        wet_bulb_globe_temperature = new Amount(
            _currentConditions.wet_bulb_globe_temperature, tempestRedStarMapping.units_temp
        ).ConvertedTo(_settings.TemperatureUnit).Value;

        wet_bulb_temperature = new Amount(
            _currentConditions.wet_bulb_temperature, tempestRedStarMapping.units_temp
        ).ConvertedTo(_settings.TemperatureUnit).Value;

        wind_avg = new Amount(_currentConditions.wind_avg, tempestRedStarMapping.units_wind)
            .ConvertedTo(_settings.WindspeedUnit).Value;

        wind_gust = new Amount(
            _currentConditions.wind_gust, tempestRedStarMapping.units_wind
        ).ConvertedTo(_settings.WindspeedUnit).Value;
    }
    public double air_density => _currentConditions.air_density;
    public double air_temperature { get; private set; }
    public double brightness => _currentConditions.brightness;
    public string conditions => _currentConditions.conditions;
    public double delta_t => _currentConditions.delta_t;
    public double dew_point { get; private set; }
    public double feels_like { get; private set; }
    public string icon => _currentConditions.icon;
    public bool is_precip_local_day_rain_check => _currentConditions.is_precip_local_day_rain_check;
    public bool is_precip_local_yesterday_rain_check => _currentConditions.is_precip_local_yesterday_rain_check;
    public double lightning_strike_count_last_1hr => _currentConditions.lightning_strike_count_last_1hr;
    public double lightning_strike_count_last_3hr => _currentConditions.lightning_strike_count_last_3hr;
    public double precip_accum_local_day { get; private set; }
    public double precip_accum_local_yesterday { get; private set; }
    public double precip_minutes_local_day => _currentConditions.precip_minutes_local_day;
    public double precip_minutes_local_yesterday => _currentConditions.precip_minutes_local_yesterday;
    public double precip_probability => _currentConditions.precip_probability;
    public string pressure_trend => _currentConditions.pressure_trend;
    public double relative_humidity => _currentConditions.relative_humidity;
    public double sea_level_pressure { get; private set; }
    public double solar_radiation => _currentConditions.solar_radiation;
    public double station_pressure { get; private set; }
    public DateTime time => Constants.UnixSecondsToDateTime(_currentConditions.time);
    public double uv => _currentConditions.uv;
    public double wet_bulb_globe_temperature { get; private set; }
    public double wet_bulb_temperature { get; private set; }
    public double wind_avg { get; private set; }
    public string wind_direction_cardinal => _currentConditions.wind_direction_cardinal;
    public double wind_gust { get; private set; }
}
