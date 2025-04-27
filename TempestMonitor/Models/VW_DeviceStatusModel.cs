namespace TempestMonitor.Models;
[Table("VW_DeviceStatus")]
public class VW_DeviceStatusModel : DatabaseBaseModel
{
    public static readonly DictionaryOfStringUnit PropertyUnit = new();
    static VW_DeviceStatusModel()
    {
        PropertyUnit.Add(nameof(voltage), ElectricUnits.Volt);
    }
    [Column("debug")]
    public long Debug { get; set; }
    [Column("firmware_revision")]
    public long firmware_revision { get; set; }
    [Column("hub_rssi")]
    public long hub_rssi { get; set; }
    [Column("hub_sn")]
    public string hub_sn { get; set; } = string.Empty;
    [Column("rssi")]
    public long rssi { get; set; }
    [Column("sensor_status")]
    public long sensor_status { get; set; }
    [Column("serial_number")]
    public string serial_number { get; set; } = string.Empty;
    [Column("timestamp_local")]
    public long timestamp_local { get; set; }
    [Column("type")]
    public string type { get; set; } = string.Empty;
    [Column("uptime")]
    public long uptime { get; set; }
    [Column("voltage")]
    public long voltage { get; set; }
}
