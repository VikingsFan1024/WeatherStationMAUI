using SQLite;
using JsonElement = System.Text.Json.JsonElement;

namespace TempestMonitor.Models;

[Table("Daily")]
public class DailyModel : ForecastChildModel
{
    [Column("air_temp_high")]
    public long AirTempHigh { get; set; }
    [Column("air_temp_low")]
    public long AirTempLow { get; set; }
    [Column("conditions")]
    public string Conditions { get; set; }
    [Column("day_num")]
    public long DayNumber { get; set; }
    [Column("day_start_local")]
    public long DayStartLocal { get; set; }
    [Column("icon")]
    public string Icon { get; set; }
    [Column("month_num")]
    public long MonthNumber { get; set; }
    [Column("precip_icon")]
    public string PrecipitationIcon { get; set; }
    [Column("precip_probability")]
    public long PrecipitationProbability { get; set; }
    [Column("precip_type")]
    public string PrecipitationType { get; set; }
    [Column("sunrise")]
    public long Sunrise { get; set; }
    [Column("sunset")]
    public long Sunset { get; set; }
    public DailyModel(ForecastModel forecast, JsonElement jsonElement) : base(forecast, jsonElement)
    {
        AirTempHigh = Constants.DoubleToLong(jsonElement.GetProperty(@"air_temp_high").GetDouble());
        AirTempLow = Constants.DoubleToLong(jsonElement.GetProperty(@"air_temp_low").GetDouble());
        Conditions = jsonElement.GetProperty(@"conditions").GetString() ?? string.Empty;
        DayNumber = jsonElement.GetProperty(@"day_num").GetInt64();
        DayStartLocal = jsonElement.GetProperty(@"day_start_local").GetInt64();
        Icon = jsonElement.GetProperty(@"icon").GetString() ?? string.Empty;
        MonthNumber = jsonElement.GetProperty(@"month_num").GetInt64();
        PrecipitationIcon = jsonElement.GetProperty(@"precip_icon").GetString() ?? string.Empty;
        PrecipitationProbability = Constants.DoubleToLong(jsonElement.GetProperty(@"precip_probability").GetDouble());
        PrecipitationType = jsonElement.GetProperty(@"precip_type").GetString() ?? string.Empty;
        Sunrise = jsonElement.GetProperty(@"sunrise").GetInt64();
        Sunset = jsonElement.GetProperty(@"sunset").GetInt64();
    }
}
