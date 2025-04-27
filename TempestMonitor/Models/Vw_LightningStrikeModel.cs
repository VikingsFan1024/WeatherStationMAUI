namespace TempestMonitor.Models;
[Table("LightningStrike")]
public class VW_LightningStrikeModel : DatabaseBaseModel
{
    public static readonly DictionaryOfStringUnit? PropertyUnit = new();
    static VW_LightningStrikeModel()
    {
        PropertyUnit.Add(nameof(distance), LengthUnits.Meter);
        PropertyUnit.Add(nameof(energy), EnergyUnits.Joule);
    }
    [Column("distance")]
    public long? distance { get; set; }
    [Column("energy")]
    public long? energy { get; set; }
    [Column("hub_sn")]
    public string? hub_sn { get; set; }
    [Column("serial_number")]
    public long? serial_number { get; set; }
    [Column("timestamp_local")]
    public long? timestamp_local { get; set; }
}
