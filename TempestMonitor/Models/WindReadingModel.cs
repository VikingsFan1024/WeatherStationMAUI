// static using for extension method classes
using static System.Linq.Enumerable; // For ToArray() by JsonElement.ArrayEnumerator

// using directives for precision in what specific classes are employed
using ColumnAttribute = SQLite.ColumnAttribute;
using DictionaryOfStringUnit = System.Collections.Generic.Dictionary<string, RedStar.Amounts.Unit>;
using SpeedUnits = RedStar.Amounts.StandardUnits.SpeedUnits;
using TableAttribute = SQLite.TableAttribute;

namespace TempestMonitor.Models;


[Table("Wind")]
public class WindReadingModel : ReadingModel
{
    public static readonly string TypeName = "rapid_wind";
    public static readonly DictionaryOfStringUnit PropertyUnit = new();
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
