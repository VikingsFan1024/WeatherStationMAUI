using TableAttribute = SQLite.TableAttribute;
using ColumnAttribute = SQLite.ColumnAttribute;
using DictionaryOfStringUnit = System.Collections.Generic.Dictionary<string, RedStar.Amounts.Unit>;
using LengthUnits = RedStar.Amounts.StandardUnits.LengthUnits;
using EnergyUnits = RedStar.Amounts.StandardUnits.EnergyUnits;
using static System.Linq.Enumerable; // For ToArray() by JsonElement.ArrayEnumerator

namespace TempestMonitor.Models;
[Table("LightningStrike")]
public class LightningStrikeModel : ReadingModel
{
    public static readonly string TypeName = "evt_strike";
    public static readonly DictionaryOfStringUnit? PropertyUnit = new();
    static LightningStrikeModel()
    {
        PropertyUnit.Add(nameof(Distance), LengthUnits.Meter);
        PropertyUnit.Add(nameof(Energy), EnergyUnits.Joule);
    }
    private enum LightingStrikeIndexes { TimestampIndex, DistanceIndex, EnergyIndex };
    [Column("Distance")]
    public long? Distance { get; set; }
    [Column("Energy")]
    public long? Energy { get; set; }
    [Column("hub_sn")]
    public string? HubSN { get; set; }
    [Column("StrikeTimestamp")]
    public long? StrikeTimestamp { get; set; }

    public LightningStrikeModel() : base()
    {
    }
    public LightningStrikeModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override LightningStrikeModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private LightningStrikeModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        HubSN = jsonElement.GetProperty(@"hub_sn").GetString() ?? string.Empty;
        var evt = jsonElement.GetProperty(@"evt").EnumerateArray().ToArray();
        StrikeTimestamp = evt[(int)LightingStrikeIndexes.TimestampIndex].GetInt64();
        Distance = evt[(int)LightingStrikeIndexes.DistanceIndex].GetInt64();
        Energy = evt[(int)LightingStrikeIndexes.EnergyIndex].GetInt64();

        return this;
    }
}
