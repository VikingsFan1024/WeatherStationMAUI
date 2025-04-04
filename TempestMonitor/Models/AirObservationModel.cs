using static System.Linq.Enumerable; // For ToArray() by JsonElement.ArrayEnumerator

using TableAttribute = SQLite.TableAttribute;
using ColumnAttribute = SQLite.ColumnAttribute;
using DictionaryOfStringUnit = System.Collections.Generic.Dictionary<string, RedStar.Amounts.Unit>;
using TemperatureUnits = RedStar.Amounts.StandardUnits.TemperatureUnits;
using ElectricUnits = RedStar.Amounts.StandardUnits.ElectricUnits;
using LengthUnits = RedStar.Amounts.StandardUnits.LengthUnits;
using TimeUnits = RedStar.Amounts.StandardUnits.TimeUnits;
using PressureUnits = RedStar.Amounts.StandardUnits.PressureUnits;

namespace TempestMonitor.Models;

[Table("AirObservation")]
public class AirObservationModel : ReadingModel, IPropertyUnit
{
    public static readonly string TypeName = "obs_air";
    public static readonly DictionaryOfStringUnit? PropertyUnit = new();

    static AirObservationModel()
    {
        PropertyUnit.Add(nameof(AirTemperature), TemperatureUnits.DegreeCelcius);
        PropertyUnit.Add(nameof(Battery), ElectricUnits.Volt);
        PropertyUnit.Add(nameof(LightningStrikeAverageDistance), LengthUnits.KiloMeter);
        PropertyUnit.Add(nameof(ReportInterval), TimeUnits.Minute);
        PropertyUnit.Add(nameof(StationPressure), PressureUnits.MilliBar);
    }
    private enum AirObservationIndexes { TimestampIndex, StationPressureIndex, AirTemperatureIndex,
        RelativeHumidityIndex, LightningStrikeCountIndex, LightningStrikeAverageDistanceIndex, BatteryIndex,
        ReportIntervalIndex
    };

    [Column("AirObservationTimestamp")]
    public long AirObservationTimestamp { get; set; }
    [Column("AirTemperature")]
    public long AirTemperature { get; set; }
    [Column("Battery")]
    public long Battery { get; set; }
    [Column("firmware_revision")]
    public long FirmwareRevision { get; set; }
    [Column("hub_sn")]
    public string HubSN { get; set; } = string.Empty;
    [Column("LightningStrikeAverageDistance")]
    public long LightningStrikeAverageDistance { get; set; }
    [Column("LightningStrikeCount")]
    public long LightningStrikeCount { get; set; }
    [Column("RelativeHumidity")]
    public long RelativeHumidity { get; set; }
    [Column("ReportInterval")]
    public long ReportInterval { get; set; }
    [Column("StationPressure")]
    public long StationPressure { get; set; }

    public AirObservationModel() : base()
    {
    }
    public AirObservationModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override AirObservationModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private AirObservationModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        HubSN = jsonElement.GetProperty(@"hub_sn").GetString() ?? string.Empty;
        var evt = jsonElement.GetProperty(@"evt").EnumerateArray().ToArray(); // ToArray by System.Linq.Enumerable
        if (evt is not null && evt.Length <= (int)AirObservationIndexes.ReportIntervalIndex)
        {
            // Not enough data in evt array to parse all fields, return empty
            return this;
        }
        AirObservationTimestamp = evt[(int)AirObservationIndexes.TimestampIndex].GetInt64();
        StationPressure = Constants.DoubleToLong(evt[(int)AirObservationIndexes.StationPressureIndex].GetDouble());
        AirTemperature = Constants.DoubleToLong(evt[(int)AirObservationIndexes.AirTemperatureIndex].GetDouble());
        RelativeHumidity = Constants.DoubleToLong(evt[(int)AirObservationIndexes.RelativeHumidityIndex].GetDouble());
        LightningStrikeCount = evt[(int)AirObservationIndexes.LightningStrikeCountIndex].GetInt64();
        LightningStrikeAverageDistance = Constants.DoubleToLong(evt[(int)AirObservationIndexes.LightningStrikeAverageDistanceIndex].GetDouble());
        Battery = Constants.DoubleToLong(evt[(int)AirObservationIndexes.BatteryIndex].GetDouble());
        ReportInterval = evt[(int)AirObservationIndexes.ReportIntervalIndex].GetInt64();
        return this;
    }
}
