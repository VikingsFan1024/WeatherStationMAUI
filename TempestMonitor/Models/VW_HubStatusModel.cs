namespace TempestMonitor.Models;
[Table("HubStatus")]
public class VW_HubStatusModel : DatabaseBaseModel
{
    [Column("serial_number")]
    public string serial_number { get; set; } = string.Empty;
    [Column("type")]
    public string type { get; set; }
    [Column("firmware_revision")]
    public string firmware_revision { get; set; } = string.Empty;
    [Column("uptime")]
    public long uptime { get; set; }
    [Column("rssi")]
    public long rssi { get; set; }
    [Column("timestamp_local")]
    public long timestamp_local { get; set; }
    [Column("reset_flags")]
    public string reset_flags { get; set; } = string.Empty;
    [Column("seq")]
    public long seq { get; set; }
    [Column("radio_stats_version")]
    public long radio_stats_version { get; set; }
    [Column("radio_stats_reboot_count")]
    public long radio_stats_reboot_count { get; set; }
    [Column("radio_stats_I2C_bus_error_count")]
    public long radio_stats_I2C_bus_error_count { get; set; }
    [Column("radio_stats_radio_status")]
    public long radio_stats_radio_status { get; set; }
    [Column("radio_stats_radio_network_id")]
    public long radio_stats_radio_network_id { get; set; }
    [Column("freq")]
    public long freq { get; set; }
    [Column("hw_version")]
    public long hw_version { get; set; }
    [Column("hardware_id")]
    public long hardware_id { get; set; }
    [Column("cellular_status")]
    public long cellular_status { get; set; }
    [Column("cell_rssi")]
    public long cell_rssi { get; set; }
}
