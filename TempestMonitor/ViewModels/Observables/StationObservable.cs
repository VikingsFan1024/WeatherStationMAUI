using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestMonitor.Models;
using RedStar.Amounts;

namespace TempestMonitor.ViewModels.Observables;

public partial class StationObservable : BaseObservable
{
    StationModel _station;
    public StationObservable(StationModel station, SettingsModel settings) : base(settings)
    {
        _station = station;
        AGL = new Amount(
            Constants.LongToDouble(station.AGL),
            station.Units.ElevationUnit)
        .ConvertedTo(_settings.ElevationUnit).Value;

        Elevation = new Amount(
            Constants.LongToDouble(station.Elevation),
            station.Units.ElevationUnit)
        .ConvertedTo(_settings.ElevationUnit).Value;
    }

    public double AGL { get; private set; }
    public double Elevation { get; private set; }
    public bool IsStationOnline => _station.IsStationOnline;
    public long State => _station.State;
    public long StationId => _station.StationId;
}
