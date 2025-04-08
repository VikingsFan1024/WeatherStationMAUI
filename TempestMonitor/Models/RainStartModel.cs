namespace TempestMonitor.Models;

[Table("RainStart")]
public class RainStartModel : ReadingModel
{
    public static readonly string TypeName = "evt_precip";
    private enum LightingStrikeIndexes { TimestampIndex };
    [Column("hub_sn")]
    public string HubSN { get; set; } = string.Empty;

    [Column("RainStartTimestamp")]
    public long RainStartTimestamp { get; set; }

    public RainStartModel() : base()
    {
    }
    public RainStartModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override RainStartModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private RainStartModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        HubSN = jsonElement.GetProperty(@"hub_sn").GetString() ?? string.Empty;
        var evt = jsonElement.GetProperty(@"evt").EnumerateArray().ToArray();
        RainStartTimestamp = evt[(int)LightingStrikeIndexes.TimestampIndex].GetInt64();

        return this;
    }
}
