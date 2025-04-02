using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using static SQLite.SQLite3;
using Serilog;
using System.Text.Json;
using RedStar.Amounts;
using RedStar.Amounts.StandardUnits;

namespace TempestMonitor.Models;
[Table("LightningStrike")]
public class LightningStrikeModel : ReadingModel
{
    public static readonly string TypeName = "evt_strike";
    public static readonly Dictionary<string, Unit>? PropertyUnit = new();
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
