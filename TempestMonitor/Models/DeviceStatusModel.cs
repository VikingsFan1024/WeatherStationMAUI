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
[Table("DeviceStatus")]
public class DeviceStatusModel : ReadingModel
{
    public static readonly string TypeName = "device_status";
    public static readonly Dictionary<string, Unit> PropertyUnit = new();
    static DeviceStatusModel()
    {
        PropertyUnit.Add(nameof(Voltage), ElectricUnits.Volt);
    }
    [Column("debug")]
    public long Debug { get; set; }
    [Column("DeviceStatusTimestamp")]
    public long DeviceStatusTimestamp { get; set; }
    [Column("firmware_revision")]
    public long FirmwareRevision { get; set; }
    [Column("hub_rssi")]
    public long HubRSSI { get; set; }
    [Column("hub_sn")]
    public string HubSN { get; set; } = string.Empty;
    [Column("rssi")]
    public long RSSI { get; set; }
    [Column("sensor_status")]
    public long SensorStatus { get; set; }
    [Column("uptime")]
    public long Uptime { get; set; }
    [Column("voltage")]
    public long Voltage { get; set; }
    public DeviceStatusModel() : base()
    {
    }
    public DeviceStatusModel(ReadingModel reading) : base(reading)
    {
        FromReadingRootElement();
    }
    public override DeviceStatusModel AssignNewValues(ReadingModel reading)
    {
        base.AssignNewValues(reading);
        return FromReadingRootElement();
    }
    private DeviceStatusModel FromReadingRootElement()
    {
        var jsonElement = base.JsonElement;
        HubSN = jsonElement.GetProperty(@"hub_sn").GetString() ?? string.Empty;
        DeviceStatusTimestamp = jsonElement.GetProperty(@"timestamp").GetInt64();
        FirmwareRevision = jsonElement.GetProperty(@"firmware_revision").GetInt64();
        Uptime = jsonElement.GetProperty(@"uptime").GetInt64();
        Voltage = Constants.DoubleToLong(jsonElement.GetProperty(@"voltage").GetDouble());
        RSSI = jsonElement.GetProperty(@"rssi").GetInt64();
        HubRSSI = jsonElement.GetProperty(@"hub_rssi").GetInt64();
        SensorStatus = jsonElement.GetProperty(@"sensor_status").GetInt64();
        Debug = jsonElement.GetProperty(@"debug").GetInt64();

        return this;
    }
}
