namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableVW_ObservationModel : ObservableObject
{
    private readonly VW_ObservationModel _observation;

    public ObservableVW_ObservationModel(VW_ObservationModel observation, SettingsModel settings) : base()
    {
        _observation = observation;

        air_temperature = new Amount(
            observation.air_temperature,
            VW_ObservationModel.PropertyUnit[nameof(_observation.air_temperature)]
        ).ConvertedTo(settings.TemperatureUnit).Value;

        lightning_strike_average_distance =
            new Amount(
                observation.lightning_strike_average_distance,
                VW_ObservationModel.PropertyUnit[nameof(_observation.lightning_strike_average_distance)]
        ).ConvertedTo(settings.DistanceUnit).Value;

        timestamp_local_formatted = Constants.UnixSecondsToDateTime(observation.timestamp_local).ToString($"MM/dd/yyyy {settings.TimeFormat}:mm:ss");

        rain_accumulation_over_the_previous_minute = new Amount(
            observation.rain_accumulation_over_the_previous_minute,
            VW_ObservationModel.PropertyUnit[nameof(_observation.rain_accumulation_over_the_previous_minute)]
        ).ConvertedTo(settings.PrecipitationUnit).Value;

        station_pressure = new Amount(
            observation.station_pressure,
            VW_ObservationModel.PropertyUnit[nameof(_observation.station_pressure)]
        ).ConvertedTo(settings.PressureUnit).Value;

        wind_average = RawToUIWind(observation.wind_average, settings);
        wind_direction_short_cardinal = Constants.GetShortCardinalDirection(observation.wind_direction);
        wind_gust = RawToUIWind(observation.wind_gust, settings);
        wind_lull = RawToUIWind(observation.wind_lull, settings);
    }

    public static double RawToUIWind(double windAverage, SettingsModel settings)
    {
        return new Amount(
            windAverage,
            VW_ObservationModel.PropertyUnit[nameof(_observation.wind_average)]
        ).ConvertedTo(settings.WindspeedUnit).Value;
    }
    public double air_temperature { get; private set; }
    public double battery => _observation.battery;
    public double illuminance => _observation.illuminance;
    public double lightning_strike_average_distance { get; private set; }
    public long lightning_strike_count => _observation.lightning_strike_count;
    public double precipitation_type => _observation.precipitation_type;
    public double rain_accumulation_over_the_previous_minute { get; private set; }
    public double relative_humidity => _observation.relative_humidity;
    public double reporting_interval => _observation.reporting_interval;
    public double solar_radiation => _observation.solar_radiation;
    public double station_pressure { get; private set; }
    public string timestamp_local_formatted { get; private set; }
    public double uv => _observation.uv;
    public double wind_average { get; private set; }
    public double wind_direction => _observation.wind_direction;
    public string wind_direction_short_cardinal { get; private set; }
    public double wind_gust { get; private set; }
    public double wind_lull { get; private set; }
    public double wind_sample_interval => _observation.wind_sample_interval;
}
