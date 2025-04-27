namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableVW_ObservationModel : ObservableObject
{
    public VW_ObservationModel Observation { get; private set; }

    public ObservableVW_ObservationModel(VW_ObservationModel observation, SettingsModel settings) : base()
    {
        Observation = observation;

        air_temperature = new Amount(
            observation.air_temperature,
            VW_ObservationModel.PropertyUnit[nameof(Observation.air_temperature)]
        ).ConvertedTo(settings.TemperatureUnit).Value;

        lightning_strike_average_distance =
            new Amount(
                observation.lightning_strike_average_distance,
                VW_ObservationModel.PropertyUnit[nameof(Observation.lightning_strike_average_distance)]
        ).ConvertedTo(settings.DistanceUnit).Value;

        timestamp_local_datetime = Constants.UnixSecondsToDateTime(observation.timestamp_local);

        rain_accumulation_over_the_previous_minute = new Amount(
            observation.rain_accumulation_over_the_previous_minute,
            VW_ObservationModel.PropertyUnit[nameof(Observation.rain_accumulation_over_the_previous_minute)]
        ).ConvertedTo(settings.PrecipitationUnit).Value;

        station_pressure = new Amount(
            observation.station_pressure,
            VW_ObservationModel.PropertyUnit[nameof(Observation.station_pressure)]
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
            VW_ObservationModel.PropertyUnit[nameof(Observation.wind_average)]
        ).ConvertedTo(settings.WindspeedUnit).Value;
    }
    public double air_temperature { get; private set; }
    public double battery => Observation.battery;
    public double illuminance => Observation.illuminance;
    public double lightning_strike_average_distance { get; private set; }
    public long lightning_strike_count => Observation.lightning_strike_count;
    public double precipitation_type => Observation.precipitation_type;
    public double rain_accumulation_over_the_previous_minute { get; private set; }
    public double relative_humidity => Observation.relative_humidity;
    public double reporting_interval => Observation.reporting_interval;
    public double solar_radiation => Observation.solar_radiation;
    public double station_pressure { get; private set; }
    public DateTime timestamp_local_datetime { get; private set; }
    public double uv => Observation.uv;
    public double wind_average { get; private set; }
    public double wind_direction => Observation.wind_direction;
    public string wind_direction_short_cardinal { get; private set; }
    public double wind_gust { get; private set; }
    public double wind_lull { get; private set; }
    public double wind_sample_interval => Observation.wind_sample_interval;
}
