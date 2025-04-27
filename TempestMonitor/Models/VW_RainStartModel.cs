namespace TempestMonitor.Models;

[Table("VW_RainStart")]
public class VW_RainStartModel : DatabaseBaseModel
{
    [Column("serial_number")]
    public string serial_number { get; set; } = string.Empty;

    [Column("hub_sn")]
    public string hub_sn { get; set; } = string.Empty;

    [Column("timestamp_local")]
    public long timestamp_local { get; set; }
}
