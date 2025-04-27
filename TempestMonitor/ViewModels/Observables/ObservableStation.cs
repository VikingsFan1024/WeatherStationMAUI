namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableStation : ObervableBase
{
    Station _station;
    public ObservableStation(
        TempestRedStarMapping tempestRedStarMapping, Station station, SettingsModel settings) : base(settings)
    {
        _station = station;
        agl = new Amount(station.agl, tempestRedStarMapping.units_elevation)
            .ConvertedTo(_settings.ElevationUnit).Value;

        elevation = new Amount(station.elevation, tempestRedStarMapping.units_elevation)
            .ConvertedTo(_settings.ElevationUnit).Value;
    }

    public double agl { get; private set; }
    public double elevation { get; private set; }
    public bool is_station_online => _station.is_station_online;
    public long state => _station.state;
    public long station_id => _station.station_id;
}
