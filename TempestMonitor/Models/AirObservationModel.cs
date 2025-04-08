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
        if (evt is null)
        {
            Log.Error($"AirObservationModel: evt was null for JsonElement {base.JsonElementString}");
            return this; // return empty object, but log the error
        }

        if (evt.Length == 0)
        {
            Log.Error($"AirObservationModel: evt array was empty for JsonElement {base.JsonElementString}");
            return this;
        }

        if (evt.Length <= (int)AirObservationIndexes.ReportIntervalIndex)
        {
            // Not enough data in evt array to parse all fields, log the error and return empty
            // evt array must have at least ReportIntervalIndex + 1 elements to parse all fields
            Log.Error($"AirObservationModel: Not enough data in evt array to parse all fields JsonElement {base.JsonElementString}");
            Log.Error($"AirObservationModel: evt array only had a length of {evt.Length}");
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
