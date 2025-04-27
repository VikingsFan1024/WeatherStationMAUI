namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableUnits(ForecastUnits units) : ObservableObject
{
    private readonly ForecastUnits _units = units;

    public string units_air_density => _units.units_air_density;
    public string units_brightness => _units.units_brightness;
    public string units_distance => _units.units_distance;
    public string units_other => _units.units_other;
    public string units_precip => _units.units_precip;
    public string units_pressure => _units.units_pressure;
    public string units_solar_radiation => _units.units_solar_radiation;
    public string units_temp => _units.units_temp;
    public string units_wind => _units.units_wind;
}
