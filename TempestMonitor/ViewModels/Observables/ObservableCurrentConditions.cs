// using directives for precision in what specific classes are employed
using Amount = RedStar.Amounts.Amount;
using CurrentConditionsModel = TempestMonitor.Models.CurrentConditionsModel;
using DateTime = System.DateTime;
using SettingsModel = TempestMonitor.Models.SettingsModel;

namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableCurrentConditions : ObervableBase
{
    private readonly CurrentConditionsModel _currentConditions;
    public ObservableCurrentConditions(CurrentConditionsModel currentConditions, SettingsModel settings) 
        : base(settings)
    {
        _currentConditions = currentConditions;
        AirDensity = Constants.LongToDouble(_currentConditions.AirDensity);
        AirTemperature = new Amount(
            Constants.LongToDouble(_currentConditions.AirTemperature), 
            currentConditions.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        DewPoint = new Amount(Constants.LongToDouble(_currentConditions.DewPoint),
            currentConditions.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        FeelsLike = new Amount(Constants.LongToDouble(_currentConditions.FeelsLike),
            currentConditions.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        PrecipitationAccumulationLocalDay = new Amount(
            Constants.LongToDouble(_currentConditions.PrecipitationAccumulationLocalDay),
            currentConditions.Units.PrecipitationUnit
        ).ConvertedTo(_settings.PrecipitationUnit).Value;
        PrecipitationAccumulationLocalYesterday = new Amount(
            Constants.LongToDouble(_currentConditions.PrecipitationAccumulationLocalYesterday),
            currentConditions.Units.PrecipitationUnit
        ).ConvertedTo(_settings.PrecipitationUnit).Value;
        SeaLevelPressure = new Amount(
            Constants.LongToDouble(_currentConditions.SeaLevelPressure),
            currentConditions.Units.PressureUnit
        ).ConvertedTo(_settings.PressureUnit).Value;
        StationPressure = new Amount(
            Constants.LongToDouble(_currentConditions.StationPressure),
            currentConditions.Units.PressureUnit
        ).ConvertedTo(_settings.PressureUnit).Value;
        WetBulbGlobeTemperature = new Amount(
            Constants.LongToDouble(_currentConditions.WetBulbGlobeTemperature),
            currentConditions.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        WetBulbTemperature = new Amount(
            Constants.LongToDouble(_currentConditions.WetBulbTemperature),
            currentConditions.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        WindAvg = new Amount(
            Constants.LongToDouble(_currentConditions.WindAvg),
            currentConditions.Units.WindspeedUnit
        ).ConvertedTo(_settings.WindspeedUnit).Value;
        WindGust = new Amount(
            Constants.LongToDouble(_currentConditions.WindGust),
            currentConditions.Units.WindspeedUnit
        ).ConvertedTo(_settings.WindspeedUnit).Value;
    }
    public double AirDensity { get; private set; }
    public double AirTemperature { get; private set; }
    public double Brightness => Constants.LongToDouble(_currentConditions.Brightness);
    public string Conditions => _currentConditions.Conditions;
    public double DeltaT => Constants.LongToDouble(_currentConditions.DeltaT);
    public double DewPoint { get; private set; }
    public double FeelsLike { get; private set; }
    public string Icon => _currentConditions.Icon;
    public bool IsPrecipLocalRainDayCheck => _currentConditions.IsPrecipLocalRainDayCheck;
    public bool IsPrecipLocalRainHourCheck => _currentConditions.IsPrecipLocalYesterdayRainCheck;
    public bool IsPrecipLocalYesterdayRainCheck => _currentConditions.IsPrecipLocalYesterdayRainCheck;
    public double LightningStrikeCountLastOneHour => _currentConditions.LightningStrikeCountLastOneHour;
    public double LightningStrikeCountLastThreeHours => _currentConditions.LightningStrikeCountLastThreeHours;
    public double PrecipitationAccumulationLocalDay { get; private set; }
    public double PrecipitationAccumulationLocalYesterday { get; private set; }
    public double PrecipitationMinutesLocalDay => Constants.LongToDouble(_currentConditions.PrecipitationMinutesLocalDay);
    public double PrecipitationMinutesLocalYesterday => Constants.LongToDouble(_currentConditions.PrecipitationMinutesLocalYesterday);
    public double PrecipitationProbability => Constants.LongToDouble(_currentConditions.PrecipitationProbability);
    public string PressureTrend => _currentConditions.PressureTrend;
    public double RelativeHumidity => Constants.LongToDouble(_currentConditions.RelativeHumidity);
    public double SeaLevelPressure { get; private set; }
    public double SolarRadiation => Constants.LongToDouble(_currentConditions.SolarRadiation);
    public double StationPressure { get; private set; }
    public DateTime Time => Constants.UnixSecondsToLocalTime(_currentConditions.Time);
    public double UV => Constants.LongToDouble(_currentConditions.UV);
    public double WetBulbGlobeTemperature { get; private set; }
    public double WetBulbTemperature { get; private set; }
    public double WindAvg { get; private set; }
    public string WindDirectionCardinal => _currentConditions.WindDirectionCardinal;
    public double WindGust { get; private set; }
}
