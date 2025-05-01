using Exception = System.Exception;          // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
namespace TempestMonitor.Models;
[Table("VW_Wind")]
public class VW_WindModel : DatabaseBaseModel_VW
{
    [Ignore]
    public static Unit WindSpeedUnit { get; } = SpeedUnits.MeterPerSecond;

    [Column("timestamp_local")]
    public long timestamp_local { get; set; }
    [Column("wind_speed")]
    public double wind_speed { get; set; }
    [Column("wind_direction")]
    public long wind_direction { get; set; }
}
