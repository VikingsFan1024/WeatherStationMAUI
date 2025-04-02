using SQLite;
using System.Text.Json;

namespace TempestMonitor.Models;

[Table("CurrentConditions")]
public class CurrentConditionsModel : ForecastChildModel
{
    [Column("air_density")]
    public long AirDensity { get; set; }
    [Column("air_temperature")]
    public long AirTemperature { get; set; }
    [Column("brightness")]
    public long Brightness { get; set; }
    [Column("conditions")]
    public string Conditions { get; set; }
    [Column("delta_t")]
    public long DeltaT { get; set; }
    [Column("dew_point")]
    public long DewPoint { get; set; }
    [Column("feels_like")]
    public long FeelsLike { get; set; }
    [Column("icon")]
    public string Icon { get; set; }
    [Column("is_precip_local_day_rain_check")]
    public bool IsPrecipLocalRainDayCheck { get; set; }
    [Column("is_precip_local_yesterday_rain_check")]
    public bool IsPrecipLocalYesterdayRainCheck { get; set; }
    [Column("lightning_strike_count_last_1hr")]
    public long LightningStrikeCountLastOneHour { get; set; }
    [Column("lightning_strike_count_last_3hr")]
    public long LightningStrikeCountLastThreeHours { get; set; }
    [Column("precip_accum_local_day")]
    public long PrecipitationAccumulationLocalDay { get; set; }
    [Column("precip_accum_local_yesterday")]
    public long PrecipitationAccumulationLocalYesterday { get; set; }
    [Column("precip_minutes_local_day")]
    public long PrecipitationMinutesLocalDay { get; set; }
    [Column("precip_minutes_local_yesterday")]
    public long PrecipitationMinutesLocalYesterday { get; set; }
    [Column("precip_probability")]
    public long PrecipitationProbability { get; set; }
    [Column("pressure_trend")]
    public string PressureTrend { get; set; }
    [Column("relative_humidity")]
    public long RelativeHumidity { get; set; }
    [Column("sea_level_pressure")]
    public long SeaLevelPressure { get; set; }
    [Column("solar_radiation")]
    public long SolarRadiation { get; set; }
    [Column("station_pressure")]
    public long StationPressure { get; set; }
    [Column("time")]
    public long Time { get; set; }
    [Column("uv")]
    public long UV { get; set; }
    [Column("wet_bulb_globe_temperature")]
    public long WetBulbGlobeTemperature { get; set; }
    [Column("wet_bulb_temperature")]
    public long WetBulbTemperature { get; set; }
    [Column("wind_avg")]
    public long WindAvg { get; set; }
    [Column("wind_direction")]
    public long WindDirection { get; set; }
    [Column("wind_direction_cardinal")]
    public string WindDirectionCardinal { get; set; }
    [Column("wind_gust")]
    public long WindGust { get; set; }

    public CurrentConditionsModel(ForecastModel forecast, JsonElement jsonElement) 
        : base(forecast, jsonElement)
    {
        AirDensity = Constants.DoubleToLong(jsonElement.GetProperty(@"air_density").GetDouble());
        AirTemperature = Constants.DoubleToLong(jsonElement.GetProperty(@"air_temperature").GetDouble());
        Brightness = Constants.DoubleToLong(jsonElement.GetProperty(@"brightness").GetDouble());
        Conditions = jsonElement.GetProperty(@"conditions").GetString() ?? string.Empty;
        DeltaT = Constants.DoubleToLong(jsonElement.GetProperty(@"delta_t").GetDouble());
        DewPoint = Constants.DoubleToLong(jsonElement.GetProperty(@"dew_point").GetDouble());
        FeelsLike = Constants.DoubleToLong(jsonElement.GetProperty(@"feels_like").GetDouble());
        Icon = jsonElement.GetProperty(@"icon").GetString() ?? string.Empty;
        IsPrecipLocalRainDayCheck = jsonElement.GetProperty(@"is_precip_local_day_rain_check").GetBoolean();
        IsPrecipLocalYesterdayRainCheck = jsonElement.GetProperty(@"is_precip_local_yesterday_rain_check").GetBoolean();
        LightningStrikeCountLastOneHour = jsonElement.GetProperty(@"lightning_strike_count_last_1hr").GetInt64();
        LightningStrikeCountLastThreeHours = jsonElement.GetProperty(@"lightning_strike_count_last_3hr").GetInt64();
        PrecipitationAccumulationLocalDay = Constants.DoubleToLong(jsonElement.GetProperty(@"precip_accum_local_day").GetDouble());
        PrecipitationAccumulationLocalYesterday = Constants.DoubleToLong(jsonElement.GetProperty(@"precip_accum_local_yesterday").GetDouble());
        PrecipitationMinutesLocalDay = jsonElement.GetProperty(@"precip_minutes_local_day").GetInt64();
        PrecipitationMinutesLocalYesterday = jsonElement.GetProperty(@"precip_minutes_local_yesterday").GetInt64();
        PrecipitationProbability = Constants.DoubleToLong(jsonElement.GetProperty(@"precip_probability").GetDouble());
        PressureTrend = jsonElement.GetProperty(@"pressure_trend").GetString() ?? string.Empty;
        RelativeHumidity = Constants.DoubleToLong(jsonElement.GetProperty(@"relative_humidity").GetDouble());
        SeaLevelPressure = Constants.DoubleToLong(jsonElement.GetProperty(@"sea_level_pressure").GetDouble());
        SolarRadiation = Constants.DoubleToLong(jsonElement.GetProperty(@"solar_radiation").GetDouble());
        StationPressure = Constants.DoubleToLong(jsonElement.GetProperty(@"station_pressure").GetDouble());
        Time = jsonElement.GetProperty(@"time").GetInt64();
        UV = Constants.DoubleToLong(jsonElement.GetProperty(@"uv").GetDouble());
        WetBulbGlobeTemperature = Constants.DoubleToLong(jsonElement.GetProperty(@"wet_bulb_globe_temperature").GetDouble());
        WetBulbTemperature = Constants.DoubleToLong(jsonElement.GetProperty(@"wet_bulb_temperature").GetDouble());
        WindAvg = Constants.DoubleToLong(jsonElement.GetProperty(@"wind_avg").GetDouble());
        WindDirection = jsonElement.GetProperty(@"wind_direction").GetInt64();
        WindDirectionCardinal = jsonElement.GetProperty(@"wind_direction_cardinal").GetString() ?? string.Empty;
        WindGust = Constants.DoubleToLong(jsonElement.GetProperty(@"wind_gust").GetDouble());
    }
}
