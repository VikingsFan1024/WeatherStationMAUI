using PressureUnitTypes = RedStar.Amounts.StandardUnits.PressureUnits;      // Naming conflict between property and PressureUnits in RedStar.Amounts.StandardUnits
using SpeedUnitTypes = RedStar.Amounts.StandardUnits.SpeedUnits;            // Naming conflict between property and SpeedUnits in RedStar.Amounts.StandardUnits
using TemperatureUnitTypes = RedStar.Amounts.StandardUnits.TemperatureUnits;// Naming conflict between property and TemperatureUnits in RedStar.Amounts.StandardUnits

namespace TempestMonitor.Models;

[Table("Units")]
public class UnitsModel : ForecastChildModel
{
    [Ignore]
    public Unit ElevationUnit => OtherUnits == @"metricc" ? LengthUnits.Meter : LengthUnits.Foot;
    [Ignore]
    public Unit AirDensityUnit => Unit.None;
    [Ignore]
    public Unit BrightnessUnit => Unit.None;
    [Ignore]
    public Unit DistanceUnit => DistanceUnits == @"km" ? LengthUnits.KiloMeter : Unit.None;
    [Ignore]
    public Unit PrecipitationUnit => PrecipitationUnits == @"mm" ? LengthUnits.MilliMeter : Unit.None;
    [Ignore]
    public Unit PressureUnit => PressureUnits == @"mb" ? PressureUnitTypes.MilliBar : Unit.None;
    [Ignore]
    public Unit SolarRadiationUnit => Unit.None;
    [Ignore]
    public Unit TemperatureUnit => TemperatureUnits == @"c" ? TemperatureUnitTypes.DegreeCelcius : Unit.None;
    [Ignore]
    public Unit WindspeedUnit => SpeedUnits == @"mps" ? SpeedUnitTypes.MeterPerSecond : Unit.None;
    [Column("units_air_density")]
    public string AirDensityUnits { get; set; }
    [Column("units_brightness")]
    public string BrightnessUnits { get; set; }
    [Column("units_distance")]
    public string DistanceUnits { get; set; }
    [Column("units_other")]
    public string OtherUnits { get; set; }
    [Column("units_precip")]
    public string PrecipitationUnits { get; set; }
    [Column("units_pressure")]
    public string PressureUnits { get; set; }
    [Column("units_solar_radiation")]
    public string SolarRadiationUnits { get; set; }
    [Column("units_temp")]
    public string TemperatureUnits { get; set; }
    [Column("units_wind")]
    public string SpeedUnits { get; set; }
    public UnitsModel(ForecastModel forecast, JsonElement unitsJsonElement) : base(forecast, unitsJsonElement)
    { 
        JsonElementString = unitsJsonElement.GetRawText();
        AirDensityUnits = unitsJsonElement.GetProperty(@"units_air_density").ToString();
        BrightnessUnits = unitsJsonElement.GetProperty(@"units_brightness").ToString();
        DistanceUnits = unitsJsonElement.GetProperty(@"units_distance").GetString() ?? string.Empty;
        OtherUnits = unitsJsonElement.GetProperty(@"units_other").GetString() ?? string.Empty;
        PrecipitationUnits = unitsJsonElement.GetProperty(@"units_precip").GetString() ?? string.Empty;
        PressureUnits = unitsJsonElement.GetProperty(@"units_pressure").GetString() ?? string.Empty;
        SolarRadiationUnits = unitsJsonElement.GetProperty(@"units_solar_radiation").GetString() ?? string.Empty;
        TemperatureUnits = unitsJsonElement.GetProperty(@"units_temp").GetString() ?? string.Empty;
        SpeedUnits = unitsJsonElement.GetProperty(@"units_wind").GetString() ?? string.Empty;
    }
}