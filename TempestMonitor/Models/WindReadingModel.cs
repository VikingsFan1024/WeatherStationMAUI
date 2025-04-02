using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SQLite;
using static SQLite.SQLite3;
using Serilog;
using RedStar.Amounts;
using RedStar.Amounts.StandardUnits;

namespace TempestMonitor.Models;


[Table("Wind")]
public class WindReadingModel : ReadingModel
{
    public static readonly string TypeName = "rapid_wind";
    public static readonly Dictionary<string, Unit> PropertyUnit = new();
    static WindReadingModel()
    {
        PropertyUnit.Add(nameof(Windspeed), SpeedUnits.MeterPerSecond);
    }
    private enum WindIndexes { TimestampIndex, SpeedIndex, DirectionDegreesIndex };
    [Column("hub_sn")]
    public string HubSN { get; set; } = string.Empty;
    [Column("WindDirection")]
    public long WindDirection { get; set; }
    [Column("Windspeed")]
    public long Windspeed { get; set; }
    [Column("WindTimestamp")]
    public long WindTimestamp { get; set; }
    public WindReadingModel() : base()
    {
    }
    public WindReadingModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override WindReadingModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private WindReadingModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        HubSN = jsonElement.GetProperty(@"hub_sn").GetString() ?? string.Empty;
        var ob = jsonElement.GetProperty(@"ob").EnumerateArray().ToArray();
        WindTimestamp = ob[(int)WindIndexes.TimestampIndex].GetInt64();
        Windspeed = Constants.DoubleToLong(ob[(int)WindIndexes.SpeedIndex].GetDouble());
        WindDirection = Constants.DoubleToLong(ob[(int)WindIndexes.DirectionDegreesIndex].GetDouble());

        return this;
    }
}
