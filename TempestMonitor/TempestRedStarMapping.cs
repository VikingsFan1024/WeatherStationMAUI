namespace TempestMonitor;
// ToDo: Change this so not being created per ForecastUnits
public class TempestRedStarMapping
{
    public TempestRedStarMapping(ForecastUnits forecastUnits)
    {
        // units_air_density = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_air_density));
        // units_brightness = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_brightness));
        units_distance = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_distance));
        // units_other = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_other));
        units_precip = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_precip));
        units_pressure = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_pressure));
        //units_solar_radiation = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_solar_radiation));
        units_temp = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_temp));
        units_wind = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_wind));
        units_elevation = UnitManager.GetUnitByName(TempestToRedStarName(forecastUnits.units_elevation));
    }
    private static string TempestToRedStarName(string unit)
    {
        return unit switch
        {
            "c" => "degree celcius",
            "f" => "degree fahrenheit",
            "K" => "Kelvin",
            "in" => "inch",
            "mm" => "millimeter",
            "hPa" => "hectopascal",
            "mb" => "millibar",
            "km" => "kilometer",
            "mps" => "meter/second",
            "m" => "meter",
            _ => unit
        };
    }

    public Unit? units_air_density => null;
    public Unit? units_brightness => null;
    public Unit units_distance { get; private set; }
    public Unit? units_other => null;
    public Unit units_precip { get; private set; }
    public Unit units_pressure { get; private set; }
    public Unit? units_solar_radiation => null;
    public Unit units_temp { get; private set; }
    public Unit units_wind { get; private set; }
    public Unit units_elevation { get; private set; }
}
