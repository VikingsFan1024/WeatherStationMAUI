namespace TempestMonitor.Models;

[Table("Status")]
public class StatusModel : ForecastChildModel
{
    [Column("status_code")]
    public long? StatusCode { get; set; }
    [Column("status_message")]
    public string? StatusMessage { get; set; }
    public StatusModel(ForecastModel forecast, JsonElement statusJsonElement) : base(forecast, statusJsonElement)
    {
        JsonElementString = statusJsonElement.GetRawText();
        StatusCode = statusJsonElement.GetProperty(@"status_code").GetInt64();
        StatusMessage = statusJsonElement.GetProperty(@"status_message").GetString();
    }
}
