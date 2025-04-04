using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;
using UnitsModel = TempestMonitor.Models.UnitsModel;

namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableUnits(UnitsModel units) : ObservableObject
{
    private readonly UnitsModel _units = units;

    public string UnitsAirDensity => _units.AirDensityUnits;
    public string UnitsBrightness => _units.BrightnessUnits;
    public string UnitsDistance => _units.DistanceUnits;
    public string UnitsOther => _units.OtherUnits;
    public string UnitsPrecip => _units.PrecipitationUnits;
    public string UnitsPressure => _units.PressureUnits;
    public string UnitsSolarRadiation => _units.SolarRadiationUnits;
    public string UnitsTemp => _units.TemperatureUnits;
    public string UnitsWind => _units.SpeedUnits;
}
