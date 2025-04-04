using ColumnAttribute = SQLite.ColumnAttribute;
using IgnoreAttribute = SQLite.IgnoreAttribute;
using TableAttribute = SQLite.TableAttribute;
using PrimaryKey = SQLite.PrimaryKeyAttribute;
using JsonElement = System.Text.Json.JsonElement;
using Guid = System.Guid;
using static System.Linq.Enumerable; 
using DateTimeOffset = System.DateTimeOffset;

namespace TempestMonitor.Models;

[Table("Forecast")]
public class ForecastModel
{
    [PrimaryKey]
    [Column("Id")]
    public string Id { get; set; }
    [Column("JsonElementString")]
    public string? JsonElementString { get; set; }
    [Column("latitude")]
    public long Latitude { get; set; }
    [Column("location_name")]
    public string LocationName { get; set; }
    [Column("longitude")]
    public long Longitude { get; set; }
    [Column("source_id_conditions")]
    public long SourceIdConditions { get; set; }
    [Column("Timestamp")]
    public long Timestamp { get; set; }
    [Column("timezone")]
    public string Timezone { get; set; }
    [Column("timezone_offset_minutes")]
    public long TimezoneOffsetMinutes { get; set; }
    [Ignore]    
    public JsonElement JsonElement { get; private set; }
    [Ignore]
    public CurrentConditionsModel CurrentConditions { get; set; }
    [Ignore]
    public StationModel Station { get; set; }
    [Ignore]
    public StatusModel Status { get; set; }
    [Ignore]
    public UnitsModel Units { get; set; }
    [Ignore]
    public DailyModel[] Dailies { get; set; }
    [Ignore]
    public HourlyModel[] Hourlies { get; set; }
    public ForecastModel(JsonElement jsonElement)
    {
        Id = Guid.NewGuid().ToString();
        JsonElement = jsonElement;
        JsonElementString = jsonElement.GetRawText();
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

        Units = new UnitsModel(this, jsonElement.GetProperty(@"units"));
        CurrentConditions = new CurrentConditionsModel(this, jsonElement.GetProperty(@"current_conditions"));
        Station = new StationModel(this, jsonElement.GetProperty(@"station"));
        Status = new StatusModel(this, jsonElement.GetProperty(@"status"));

        Latitude = Constants.DoubleToLong(jsonElement.GetProperty(@"latitude").GetDouble());
        LocationName = jsonElement.GetProperty(@"location_name").GetString() ?? string.Empty;
        Longitude = Constants.DoubleToLong(jsonElement.GetProperty(@"longitude").GetDouble());
        SourceIdConditions = jsonElement.GetProperty(@"source_id_conditions").GetInt64();
        Timezone = jsonElement.GetProperty(@"timezone").GetString() ?? string.Empty;
        TimezoneOffsetMinutes = jsonElement.GetProperty(@"timezone_offset_minutes").GetInt64();
        var forecastJsonElement = jsonElement.GetProperty(@"forecast");
        var dailyJsonElement = forecastJsonElement.GetProperty(@"daily");
        Dailies = dailyJsonElement
            .EnumerateArray()
            .Select(dailyJsonElement => new DailyModel(this, dailyJsonElement))
            .ToArray();
        var hourlyJsonElement = forecastJsonElement.GetProperty(@"hourly");
        Hourlies = hourlyJsonElement
            .EnumerateArray()
            .OrderBy(hourlyJsonElement => hourlyJsonElement.GetProperty(@"time").GetInt64())
            .Take(Constants.NumberOfHoursInForecastToKeep)
            .Select(hourlyJsonElement => new HourlyModel(this, hourlyJsonElement))
            .ToArray();
    }
}