using TableAttribute = SQLite.TableAttribute;
using ColumnAttribute = SQLite.ColumnAttribute;
using JsonElement = System.Text.Json.JsonElement;

namespace TempestMonitor.Models;

[Table("Station")]
public class StationModel : ForecastChildModel
{
    [Column("agl")]
    public long AGL { get; set; }
    [Column("elevation")]
    public long Elevation { get; set; }
    [Column("is_station_online")]
    public bool IsStationOnline { get; set; }
    [Column("state")]
    public long State { get; set; }
    [Column("station_id")]
    public long StationId { get; set; }
    public StationModel(ForecastModel forecast, JsonElement jsonElement) : base(forecast, jsonElement)
    {
        AGL = Constants.DoubleToLong(jsonElement.GetProperty(@"agl").GetDouble());
        Elevation = Constants.DoubleToLong(jsonElement.GetProperty(@"elevation").GetDouble());
        IsStationOnline = jsonElement.GetProperty(@"is_station_online").GetBoolean();
        State = jsonElement.GetProperty(@"state").GetInt64();
        StationId = jsonElement.GetProperty(@"station_id").GetInt64();
    }
}
