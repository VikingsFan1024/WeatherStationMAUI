// static using for extension method classes
using static System.Linq.Enumerable; // For ToArray() by JsonElement.ArrayEnumerator

// Aliases for types used in this file to keep the code cleaner
using DictionaryOfStringUnit = System.Collections.Generic.Dictionary<string, RedStar.Amounts.Unit>;

// using directives for precision in what specific classes are employed
using ColumnAttribute = SQLite.ColumnAttribute;
using ElectricUnits = RedStar.Amounts.StandardUnits.ElectricUnits;
using LengthUnits = RedStar.Amounts.StandardUnits.LengthUnits;
using SpeedUnits = RedStar.Amounts.StandardUnits.SpeedUnits;
using TableAttribute = SQLite.TableAttribute;
using TimeUnits = RedStar.Amounts.StandardUnits.TimeUnits;

namespace TempestMonitor.Models;
[Table("SkyObservation")]
public class SkyObservationModel : ReadingModel
{
    public static readonly string TypeName = "obs_sky";
    public static readonly DictionaryOfStringUnit? PropertyUnit = new();
    static SkyObservationModel()
    {
        PropertyUnit.Add(nameof(Battery), ElectricUnits.Volt);
        PropertyUnit.Add(nameof(LocalDayRainAccumulation), LengthUnits.MilliMeter);
        PropertyUnit.Add(nameof(RainAccumulationOverThePreviousMinute), LengthUnits.MilliMeter);
        PropertyUnit.Add(nameof(ReportInterval), TimeUnits.Minute);
        PropertyUnit.Add(nameof(WindAverage), SpeedUnits.MeterPerSecond);
        PropertyUnit.Add(nameof(WindGust), SpeedUnits.MeterPerSecond);
        PropertyUnit.Add(nameof(WindLull), SpeedUnits.MeterPerSecond);
    }
    private enum SkyObservationIndexes { TimestampIndex, IlluminanceIndex, UVIndex, 
        RainAccumulationOverThePreviousMinuteIndex, WindLullIndex, WindAverageIndex, WindGustIndex, 
        WindDirectionDegreesIndex, BatteryIndex, ReportingIntervalIndex, SolarRadiationIndex,
        LocalDayRainAccumulationIndex, PrecipitationTypeIndex, WindSampleIntervalIndex
    };
    private enum PrecipitationTypes
    {
        NoPrecipitation = 0,
        Rain = 1,
        Hail = 2,
        RainAndHail = 3
    }

    [Column("Battery")]
    public long? Battery { get; set; }
    [Column("firmware_revision")]
    public long? FirmwareRevision { get; set; }
    [Column("hub_sn")]
    public string? HubSN { get; set; }
    [Column("Illuminance")]
    public long? Illuminance { get; private set; }
    [Column("LocalDayRainAccumulation")]
    public long? LocalDayRainAccumulation { get; set; }
    [Column("PrecipitationType")]
    public long? PrecipitationType { get; set; }
    [Column("RainAccumulationOverThePreviousMinute")]
    public long? RainAccumulationOverThePreviousMinute { get; private set; }
    [Column("ReportInterval")]
    public long? ReportInterval { get; set; }
    [Column("SkyObservationTimestamp")]
    public long? SkyObservationTimestamp { get; set; }
    [Column("SolarRadiation")]
    public long? SolarRadiation { get; set; }
    [Column("UV")]
    public long? UV { get; private set; }
    [Column("WindAverage")]
    public long? WindAverage { get; private set; }
    [Column("WindDirectionDegrees")]
    public long? WindDirectionDegrees { get; private set; }
    [Column("WindGust")]
    public long? WindGust { get; private set; }
    [Column("WindLull")]
    public long? WindLull { get; private set; }
    [Column("WindSampleInterval")]
    public long? WindSampleInterval { get; set; }

    public SkyObservationModel() : base()
    {
    }
    public SkyObservationModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override SkyObservationModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private SkyObservationModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        HubSN = jsonElement.GetProperty(@"hub_sn").GetString() ?? string.Empty;
        var evt = jsonElement.GetProperty(@"evt").EnumerateArray().ToArray();
        SkyObservationTimestamp = evt[(int)SkyObservationIndexes.TimestampIndex].GetInt64();
        Illuminance = evt[(int)SkyObservationIndexes.IlluminanceIndex].GetInt64();
        UV = Constants.DoubleToLong(evt[(int)SkyObservationIndexes.UVIndex].GetDouble());
        RainAccumulationOverThePreviousMinute = Constants.DoubleToLong(
            evt[(int)SkyObservationIndexes.RainAccumulationOverThePreviousMinuteIndex].GetDouble());
        WindLull = Constants.DoubleToLong(
            evt[(int)SkyObservationIndexes.WindLullIndex].GetDouble());
        WindAverage = Constants.DoubleToLong(
            evt[(int)SkyObservationIndexes.WindAverageIndex].GetDouble());
        WindGust = Constants.DoubleToLong(
            evt[(int)SkyObservationIndexes.WindGustIndex].GetDouble());
        WindDirectionDegrees = Constants.DoubleToLong(
            evt[(int)SkyObservationIndexes.WindDirectionDegreesIndex].GetInt64());
        Battery = Constants.DoubleToLong(evt[(int)SkyObservationIndexes.BatteryIndex].GetDouble());
        ReportInterval = evt[(int)SkyObservationIndexes.ReportingIntervalIndex].GetInt64();
        SolarRadiation = Constants.DoubleToLong(
            evt[(int)SkyObservationIndexes.SolarRadiationIndex].GetDouble());
        LocalDayRainAccumulation = Constants.DoubleToLong(
            evt[(int)SkyObservationIndexes.LocalDayRainAccumulationIndex].GetDouble());
        PrecipitationType = evt[(int)SkyObservationIndexes.PrecipitationTypeIndex].GetInt64();
        WindSampleInterval = evt[(int)SkyObservationIndexes.WindSampleIntervalIndex].GetInt64();

        return this;
    }
}
