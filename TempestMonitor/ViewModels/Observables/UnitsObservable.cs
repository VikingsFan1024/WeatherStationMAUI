using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestMonitor.Models;

namespace TempestMonitor.ViewModels.Observables;

public partial class UnitsObservable(UnitsModel units) : ObservableObject
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
