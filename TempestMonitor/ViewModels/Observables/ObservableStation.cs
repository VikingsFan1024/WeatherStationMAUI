using Amount = RedStar.Amounts.Amount;
using SettingsModel = TempestMonitor.Models.SettingsModel;
using StationModel = TempestMonitor.Models.StationModel;

namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableStation : ObervableBase
{
    StationModel _station;
    public ObservableStation(StationModel station, SettingsModel settings) : base(settings)
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
