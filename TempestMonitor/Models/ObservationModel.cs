namespace TempestMonitor.Models;

[Table("Observation")]
public class ObservationModel : ReadingModel
{
    public static readonly string TypeName = "obs_st";
    public static readonly DictionaryOfStringUnit PropertyUnit = new();
    static ObservationModel()
    {
        PropertyUnit.Add(nameof(AirTemperature), TemperatureUnits.DegreeCelcius);
        PropertyUnit.Add(nameof(Battery), ElectricUnits.Volt);
        PropertyUnit.Add(nameof(LightningStrikeAverageDistance), LengthUnits.KiloMeter);
        PropertyUnit.Add(nameof(RainAccumulationOverThePreviousMinute), LengthUnits.MilliMeter);
        PropertyUnit.Add(nameof(ReportInterval), TimeUnits.Minute);
        PropertyUnit.Add(nameof(StationPressure), PressureUnits.MilliBar);
        PropertyUnit.Add(nameof(WindAverage), SpeedUnits.MeterPerSecond);
        PropertyUnit.Add(nameof(WindGust), SpeedUnits.MeterPerSecond);
        PropertyUnit.Add(nameof(WindLull), SpeedUnits.MeterPerSecond);
    }

    private static readonly byte AirTemperatureIndex = 7;
    private static readonly byte AverageLightningStrikeDistanceIndex = 14;
    private static readonly byte BatteryIndex = 16;
    private static readonly byte IllumnanceIndex = 9;
    private static readonly byte LightningStrikeCountIndex = 15;
    private const byte ObservationTimestampIndex = 0;
    private static readonly byte PrecipationTypeIndex = 13;
    private static readonly byte RainAccumulationOverThePreviousMinuteIndex = 12;
    private static readonly byte RelativeHumidityIndex = 8;
    private static readonly byte ReportIntervalIndex = 17;
    private static readonly byte SolarRadiationIndex = 11;
    private static readonly byte StationPressureIndex = 6;
    private static readonly byte UVArrayIndex = 10;
    private static readonly byte WindAverageIndex = 2;
    private static readonly byte WindDirectionIndex = 4;
    private static readonly byte WindGustIndex = 3;
    private static readonly byte WindLullIndex = 1;
    private static readonly byte WindSampleIntervalIndex = 5;

    [Column("AirTemperature")]
    public long AirTemperature { get; set; }
    [Column("Battery")]
    public long Battery { get; set; }
    [Column("firmware_revision")]
    public long FirmwareRevision { get; set; }
    [Column("hub_sn")]
    public string HubSN { get; set; } = string.Empty;
    [Column("Illuminance")]
    public long Illuminance { get; set; }
    [Column("LightningStrikeAverageDistance")]
    public long LightningStrikeAverageDistance { get; set; }
    [Column("LightningStrikeCount")]
    public long LightningStrikeCount { get; set; }
    [Column("ObservationTimestamp")]
    public long ObservationTimestamp { get; set; }
    [Column("PrecipitationType")]
    public long PrecipitationType { get; set; }
    [Column("RainAccumulationOverThePreviousMinute")]
    public long RainAccumulationOverThePreviousMinute { get; set; }
    [Column("RelativeHumidity")]
    public long RelativeHumidity { get; set; }
    [Column("ReportInterval")]
    public long ReportInterval { get; set; }
    [Column("SolarRadiation")]
    public long SolarRadiation { get; set; }
    [Column("StationPressure")]
    public long StationPressure { get; set; }
    [Column("UV")]
    public long UV { get; set; }
    [Column("WindAverage")]
    public long WindAverage { get; set; }
    [Column("WindDirection")]   
    public long WindDirection { get; set; }
    [Column("WindGust")]
    public long WindGust { get; set; }
    [Column("WindLull")]
    public long WindLull { get; set; }
    [Column("WindSampleInterval")]
    public long WindSampleIntervalInMinutes { get; set; }

    public ObservationModel() : base()
    {

    }
    public ObservationModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override ObservationModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private ObservationModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        HubSN = jsonElement.GetProperty(@"hub_sn").GetString() ?? string.Empty;
        var obs = jsonElement.GetProperty(@"obs").EnumerateArray().Select(x => x.EnumerateArray().Select(y => y.GetDouble()).ToArray()).ToArray();
        FirmwareRevision = jsonElement.GetProperty(@"firmware_revision").GetInt32();
        var observationArray = jsonElement.GetProperty(@"obs");
        foreach (var observation in observationArray.EnumerateArray())
        {
            ObservationTimestamp = observation[ObservationTimestampIndex].GetInt64();
            WindLull = Constants.DoubleToLong(observation[WindLullIndex].GetDouble());
            WindAverage = Constants.DoubleToLong(observation[WindAverageIndex].GetDouble());
            WindGust = Constants.DoubleToLong(observation[WindGustIndex].GetDouble());
            WindDirection = Constants.DoubleToLong(observation[WindDirectionIndex].GetDouble());
            WindSampleIntervalInMinutes = observation[WindSampleIntervalIndex].GetInt64();
            StationPressure = Constants.DoubleToLong(observation[StationPressureIndex].GetDouble());
            AirTemperature = Constants.DoubleToLong(observation[AirTemperatureIndex].GetDouble());
            RelativeHumidity = Constants.DoubleToLong(observation[RelativeHumidityIndex].GetDouble());
            Illuminance = Constants.DoubleToLong(observation[IllumnanceIndex].GetDouble());
            UV = Constants.DoubleToLong(observation[UVArrayIndex].GetDouble());
            SolarRadiation = Constants.DoubleToLong(observation[SolarRadiationIndex].GetDouble());
            RainAccumulationOverThePreviousMinute = Constants.DoubleToLong(observation[RainAccumulationOverThePreviousMinuteIndex].GetDouble());
            PrecipitationType = observation[PrecipationTypeIndex].GetInt64();
            LightningStrikeAverageDistance = Constants.DoubleToLong(observation[AverageLightningStrikeDistanceIndex].GetDouble());
            LightningStrikeCount = observation[LightningStrikeCountIndex].GetInt64();
            Battery = Constants.DoubleToLong(observation[BatteryIndex].GetDouble());
            ReportInterval = Constants.DoubleToLong(observation[ReportIntervalIndex].GetDouble());
        }

        return this;
    }
}
