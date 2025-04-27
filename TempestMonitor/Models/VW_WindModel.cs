namespace TempestMonitor.Models;
[Table("VW_Wind")]
public class VW_WindModel : DatabaseBaseModel
{
    [Ignore]
    public static Unit WindSpeedUnit { get; } = SpeedUnits.MeterPerSecond;

    [Column("type")]
    public string type { get; set; } = string.Empty;
    [Column("serial_number")]
    public string serial_number { get; set; } = string.Empty;
    [Column("hub_sn")]
    public string hub_sn { get; set; } = string.Empty;
    [Column("timestamp_local")]
    public long timestamp_local { get; set; }
    [Column("wind_speed")]
    public double wind_speed { get; set; }
    [Column("wind_direction")]
    public long wind_direction { get; set; }
}
