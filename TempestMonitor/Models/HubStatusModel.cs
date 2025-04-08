namespace TempestMonitor.Models;
[Table("HubStatus")]
public class HubStatusModel : ReadingModel
{
    public static readonly string TypeName = "hub_status";
    public static string NativeTimeName => @"minute";
    private enum RadioStatsIndexes
    {
        VersionIndex, RebootCountIndex, I2CBusErrorCountIndex, RadioStatusIndex, RadioNetworkId
    };

    private enum RadioStatusValues
    {
        RadioOff = 0, RadioOn = 1, RadioActive = 3, BleConnected = 7
    };

    [Column("firmware_revision")]
    public string FirmwareRevision { get; set; } = string.Empty;
    [Column("HubStatusTimestamp")]
    public long HubStatusTimestamp { get; set; }
    [Column("I2CBusErrorCount")]
    public long I2CBusErrorCount { get; set; }
    [Column("reset_flags")]
    public string ResetFlags { get; set; } = string.Empty;
    [Column("rssi")]
    public long RSSI { get; set; }
    [Column("RadioNetworkId")]
    public long RadioNetworkId { get; set; }
    [Column("RadioRebootCount")]
    public long RadioRebootCount { get; set; }
    [Column("RadioStatus")]
    public long RadioStatus { get; set; }
    [Column("RadioVersion")]
    public long RadioVersion { get; set; }
    [Column("seq")]
    public long Seq { get; set; }
    [Column("uptime")]
    public long Uptime { get; set; }

    public HubStatusModel() : base()
    {
    }
    public HubStatusModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override HubStatusModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private HubStatusModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        FirmwareRevision = jsonElement.GetProperty(@"firmware_revision").GetString() ?? string.Empty;
        Uptime = jsonElement.GetProperty(@"uptime").GetInt64();
        RSSI = jsonElement.GetProperty(@"rssi").GetInt64();
        HubStatusTimestamp = jsonElement.GetProperty(@"timestamp").GetInt64();
        ResetFlags = jsonElement.GetProperty(@"reset_flags").GetString() ?? string.Empty;
        Seq = jsonElement.GetProperty(@"seq").GetInt64();

        var radioStats = jsonElement.GetProperty(@"radio_stats").EnumerateArray().ToArray();
        RadioVersion = radioStats[(int)RadioStatsIndexes.VersionIndex].GetInt64();
        RadioRebootCount = radioStats[(int)RadioStatsIndexes.RebootCountIndex].GetInt64();
        I2CBusErrorCount = radioStats[(int)RadioStatsIndexes.I2CBusErrorCountIndex].GetInt64();
        RadioStatus = radioStats[(int)RadioStatsIndexes.RadioStatusIndex].GetInt64();
        RadioNetworkId = radioStats[(int)RadioStatsIndexes.RadioNetworkId].GetInt64();

        return this;
    }
}
