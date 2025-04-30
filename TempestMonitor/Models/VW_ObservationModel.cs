namespace TempestMonitor.Models;
[Table("VW_Observation")]
public class VW_ObservationModel : DatabaseBaseModel_VW
{
    public static readonly DictionaryOfStringUnit PropertyUnit = new();
    static VW_ObservationModel()
    {
        PropertyUnit.Add(nameof(air_temperature), TemperatureUnits.DegreeCelcius);
        PropertyUnit.Add(nameof(battery), ElectricUnits.Volt);
        PropertyUnit.Add(nameof(lightning_strike_average_distance), LengthUnits.KiloMeter);
        PropertyUnit.Add(nameof(rain_accumulation_over_the_previous_minute), LengthUnits.MilliMeter);
        PropertyUnit.Add(nameof(reporting_interval), TimeUnits.Minute);
        PropertyUnit.Add(nameof(station_pressure), PressureUnits.MilliBar);
        PropertyUnit.Add(nameof(wind_average), SpeedUnits.MeterPerSecond);
        PropertyUnit.Add(nameof(wind_gust), SpeedUnits.MeterPerSecond);
        PropertyUnit.Add(nameof(wind_lull), SpeedUnits.MeterPerSecond);
    }

    [Column("air_temperature")]
    public double air_temperature { get; set; }
    [Column("battery")]
    public double battery { get; set; }
    [Column("firmware_revision")]
    public long firmware_revision { get; set; }
    [Column("hub_sn")]
    public string hub_sn { get; set; } = string.Empty;
    [Column("illuminance")]
    public double illuminance { get; set; }
    [Column("lightning_strike_average_distance")]
    public double lightning_strike_average_distance { get; set; }
    [Column("lightning_strike_count")]
    public long lightning_strike_count { get; set; }
    [Column("precipitation_type")]
    public long precipitation_type { get; set; }
    [Column("rain_accumulation_over_the_previous_minute")]
    public double rain_accumulation_over_the_previous_minute { get; set; }
    [Column("relative_humidity")]
    public double relative_humidity { get; set; }
    [Column("reporting_interval")]
    public long reporting_interval { get; set; }
    [Column("solar_radiation")]
    public double solar_radiation { get; set; }
    [Column("station_pressure")]
    public double station_pressure { get; set; }
    [Column("timestamp_local")]
    public long timestamp_local { get; set; }
    [Column("uv")]
    public double uv { get; set; }
    [Column("wind_average")]
    public double wind_average { get; set; }
    [Column("wind_direction")]
    public long wind_direction { get; set; }
    [Column("wind_gust")]
    public double wind_gust { get; set; }
    [Column("wind_lull")]
    public double wind_lull { get; set; }
    [Column("wind_sample_interval")]
    public long wind_sample_interval { get; set; }
}
