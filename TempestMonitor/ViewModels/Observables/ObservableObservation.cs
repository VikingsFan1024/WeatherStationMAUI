// using directives for precision in what specific classes are employed
using Amount = RedStar.Amounts.Amount;
using CultureInfo = System.Globalization.CultureInfo;
using DateTime = System.DateTime;
using ObservationModel = TempestMonitor.Models.ObservationModel;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;
using SettingsModel = TempestMonitor.Models.SettingsModel;

namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableObservation : ObservableObject
{
    public ObservationModel Observation { get; private set; }

    public ObservableObservation(ObservationModel observation, SettingsModel settings) : base()
    {
        Observation = observation;

        AirTemperature = RawToUIAirTemperature(observation.AirTemperature, settings);

        Battery = RawToUIBattery(observation.Battery, settings);

        Illuminance = RawToUIIlluminance(observation.Illuminance, settings);

        LightningStrikeAverageDistance = RawToUILightningStrikeAverageDistance(observation.LightningStrikeAverageDistance, settings);

        LightningStrikeCount = RawToUILightningStrikeCount(observation.LightningStrikeCount, settings);

        ObservationTimestamp = RawToUIObservationTimestamp(observation.ObservationTimestamp, settings);

        ObservationTimestampTimeOnlyString = RawToUIObservationTimestampTimeOnlyString(
            observation.ObservationTimestamp, settings);

        PrecipitationType = RawToUIPrecipitationType(observation.PrecipitationType, settings);
        
        RainAccumulationOverThePreviousMinute = RawToUIRainAccumulationOverThePreviousMinute(
                observation.RainAccumulationOverThePreviousMinute, settings);

        RelativeHumidity = RawToUIRelativeHumidity(observation.RelativeHumidity, settings);

        ReportInterval = RawToUIReportInterval(observation.ReportInterval, settings);

        SolarRadiation = RawToUISolarRadiation(observation.SolarRadiation, settings);

        StationPressure = RawToUIStationPressure(observation.StationPressure, settings);

        UV = RawToUIUV(observation.UV, settings);

        WindAverage = RawToUIWind(observation.WindAverage, settings);

        WindDirection = RawToUIWindDirection(observation.WindDirection, settings);

        WindDirectionShortCardinal = RawToUIWindDirectionShortCardinal(observation.WindDirection, settings);

        WindGust = RawToUIWind(observation.WindGust, settings);

        WindLull = RawToUIWind(observation.WindLull, settings);

        WindSampleInterval = RawToUIWindSampleInterval(observation.WindSampleIntervalInMinutes, settings);
    }

    public static double RawToUIAirTemperature(long airTemperature, SettingsModel settings)
    {
        return new Amount(
            Constants.LongToDouble(airTemperature),
            ObservationModel.PropertyUnit[nameof(Observation.AirTemperature)]
        ).ConvertedTo(settings.TemperatureUnit).Value;
    }
    public static double RawToUIBattery(long battery, SettingsModel settings)
    {
        return Constants.LongToDouble(battery);
    }
    public static double RawToUIIlluminance(long illuminance, SettingsModel settings)
    {
        return Constants.LongToDouble(illuminance);
    }
    public static double RawToUILightningStrikeAverageDistance(
        long lightningStrikeAverageDistance, SettingsModel settings)
    {
        return new Amount(
            Constants.LongToDouble(lightningStrikeAverageDistance),
            ObservationModel.PropertyUnit[nameof(Observation.LightningStrikeAverageDistance)]
        ).ConvertedTo(settings.DistanceUnit).Value;
    }
    public static long RawToUILightningStrikeCount(long lightningStrikeCount, SettingsModel settings)
    {
        return lightningStrikeCount;
    }
    public static DateTime RawToUIObservationTimestamp(long observationTimestamp, SettingsModel settings)
    {
        return Constants.UnixSecondsToLocalTime(observationTimestamp);
    }
    public static string RawToUIObservationTimestampTimeOnlyString(long observationTimestamp, SettingsModel settings)
    {
        return Constants.UnixSecondsToLocalTime(observationTimestamp).
            ToString(settings.TimeFormat + ":mm:ss", CultureInfo.InvariantCulture);
    }
    public static double RawToUIPrecipitationType(long precipitationType, SettingsModel settings)
    {
        return Constants.LongToDouble(precipitationType);
    }
    public static double RawToUIRainAccumulationOverThePreviousMinute(
        long rainAccumulationOverThePreviousMinute, SettingsModel settings)
    {
        return new Amount(
            Constants.LongToDouble(rainAccumulationOverThePreviousMinute),
            ObservationModel.PropertyUnit[nameof(Observation.RainAccumulationOverThePreviousMinute)]
        ).ConvertedTo(settings.PrecipitationUnit).Value;
    }
    public static double RawToUIRelativeHumidity(long relativeHumidity, SettingsModel settings)
    {
        return Constants.LongToDouble(relativeHumidity);
    }
    public static double RawToUIReportInterval(long reportInterval, SettingsModel settings)
    {
        return Constants.LongToDouble(reportInterval);
    }
    public static double RawToUISolarRadiation(long solarRadiation, SettingsModel settings)
    {
        return Constants.LongToDouble(solarRadiation);
    }
    public static double RawToUIStationPressure(long stationPressure, SettingsModel settings)
    {
        return new Amount(
            Constants.LongToDouble(stationPressure),
            ObservationModel.PropertyUnit[nameof(Observation.StationPressure)]
        ).ConvertedTo(settings.PressureUnit).Value;
    }
    public static double RawToUIUV(long uV, SettingsModel settings)
    {
        return Constants.LongToDouble(uV);
    }
    public static double RawToUIWind(long windAverage, SettingsModel settings)
    {
        return new Amount(
            Constants.LongToDouble(windAverage),
            ObservationModel.PropertyUnit[nameof(Observation.WindAverage)]
        ).ConvertedTo(settings.WindspeedUnit).Value;
    }
    public static double RawToUIWindDirection(long windDirection, SettingsModel settings)
    {
        return Constants.LongToDouble(windDirection);
    }
    public static string RawToUIWindDirectionShortCardinal(long windDirection, SettingsModel settings)
    {
        return Constants.GetShortCardinalDirection(windDirection);
    }
    private static double RawToUIWindSampleInterval(long windSampleIntervalInMinutes, SettingsModel settings)
    {
        return windSampleIntervalInMinutes;
    }
    public double AirTemperature { get; private set; }
    public double Battery { get; private set; }
    public double Illuminance { get; private set; }
    public double LightningStrikeAverageDistance { get; private set; }
    public long LightningStrikeCount { get; private set; } 
    public DateTime ObservationTimestamp { get; private set; }
    public string ObservationTimestampTimeOnlyString { get; private set; }
    public double PrecipitationType { get; private set; }
    public double RainAccumulationOverThePreviousMinute { get; private set; }
    public double RelativeHumidity { get; private set; }
    public double ReportInterval { get; private set; }
    public double SolarRadiation { get; private set; }
    public double StationPressure { get; private set; }
    public double UV { get; private set; }
    public double WindAverage { get; private set; }
    public double WindDirection {  get; private set; }
    public string WindDirectionShortCardinal { get; private set; }
    public double WindGust { get; private set; }
    public double WindLull { get; private set; }
    public double WindSampleInterval { get; private set; }
}
