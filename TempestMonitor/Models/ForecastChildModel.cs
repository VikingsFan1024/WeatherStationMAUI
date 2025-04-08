namespace TempestMonitor.Models;

public class ForecastChildModel
{
    protected ForecastChildModel(ForecastModel forecast, JsonElement jsonElement)
    {
        Forecast = forecast;
        ForecastId = forecast.Id;
        Id = Guid.NewGuid().ToString();
        JsonElementString = jsonElement.GetRawText();
    }

    [Ignore]
    protected ForecastModel Forecast { get; set; }

    [Column("Id")]
    public string Id { get; set; }

    [Column("ForecastId")]
    public string ForecastId { get; set; }

    [Column("JsonElementString")]
    public string JsonElementString { get; set; }

    [Ignore]
    public UnitsModel Units => Forecast.Units;
}
