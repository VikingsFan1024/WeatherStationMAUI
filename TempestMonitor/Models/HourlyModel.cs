// using directives for precision in what specific classes are employed
using ColumnAttribute = SQLite.ColumnAttribute;
using JsonElement = System.Text.Json.JsonElement;
using TableAttribute = SQLite.TableAttribute;

namespace TempestMonitor.Models;

[Table("Hourly")]
public class HourlyModel : ForecastChildModel
{
    [Column("air_temperature")]
    public long AirTemperature { get; set; }
    [Column("conditions")]
    public string Conditions { get; set; }
    [Column("feels_like")]
    public long FeelsLike { get; set; }
    [Column("icon")]
    public string Icon { get; set; }
    [Column("local_day")]
    public long LocalDay { get; set; }
    [Column("local_hour")]
    public long LocalHour { get; set; }
    [Column("precip")]
    public long Precipitation { get; set; }
    [Column("precip_icon")]
    public string PrecipitationIcon { get; set; }
    [Column("precip_probability")]
    public long PrecipitationProbability { get; set; }
    [Column("precip_type")]
    public string PrecipitationType { get; set; }
    [Column("relative_humidity")]
    public long RelativeHumidity { get; set; }
    [Column("sea_level_pressure")]
    public long SeaLevelPressure { get; set; }
    [Column("station_pressure")]
    public long StationPressure { get; set; }
    [Column("time")]
    public long Time { get; set; }
    [Column("uv")]
    public long UV { get; set; }
    [Column("wind_avg")]
    public long WindAvg { get; set; }
    [Column("wind_direction")]
    public long WindDirection { get; set; }
    [Column("wind_direction_cardinal")]
    public string WindDirectionCardinal { get; set; }
    [Column("wind_gust")]
    public long WindGust { get; set; }
    public HourlyModel(ForecastModel forecast, JsonElement jsonElement) : base(forecast, jsonElement)
    {
        AirTemperature = Constants.DoubleToLong(jsonElement.GetProperty(@"air_temperature"));
        Conditions = jsonElement.GetProperty(@"conditions").GetString() ?? string.Empty;
        FeelsLike = Constants.DoubleToLong(jsonElement.GetProperty(@"feels_like").GetDouble());
        Icon = jsonElement.GetProperty(@"icon").GetString() ?? string.Empty;
        LocalDay = jsonElement.GetProperty(@"local_day").GetInt64();
        LocalHour = jsonElement.GetProperty(@"local_hour").GetInt64();
        Precipitation = Constants.DoubleToLong(jsonElement.GetProperty(@"precip").GetDouble());
        PrecipitationIcon = jsonElement.GetProperty(@"precip_icon").GetString() ?? string.Empty;
        PrecipitationProbability = Constants.DoubleToLong(jsonElement.GetProperty(@"precip_probability").GetDouble());
        PrecipitationType = jsonElement.GetProperty(@"precip_type").GetString() ?? string.Empty;
        RelativeHumidity = Constants.DoubleToLong(jsonElement.GetProperty(@"relative_humidity").GetDouble());
        SeaLevelPressure = Constants.DoubleToLong(jsonElement.GetProperty(@"sea_level_pressure").GetDouble());
        StationPressure = Constants.DoubleToLong(jsonElement.GetProperty(@"station_pressure").GetDouble());
        Time = jsonElement.GetProperty(@"time").GetInt64();
        UV = Constants.DoubleToLong(jsonElement.GetProperty(@"uv").GetDouble());
        WindAvg = Constants.DoubleToLong(jsonElement.GetProperty(@"wind_avg").GetDouble());
        WindDirection = jsonElement.GetProperty(@"wind_direction").GetInt64();
        WindDirectionCardinal = jsonElement.GetProperty(@"wind_direction_cardinal").GetString() ?? string.Empty;
        WindGust = Constants.DoubleToLong(jsonElement.GetProperty(@"wind_gust").GetDouble());
    }
}
