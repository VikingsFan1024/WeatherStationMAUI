namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableVW_WindModel : ObservableObject
{
    private readonly VW_WindModel _vw_WindModel;

    public ObservableVW_WindModel(VW_WindModel vw_WindModel, SettingsModel settings)
    {
        _vw_WindModel = vw_WindModel;
        timestamp_local_formatted = Constants.UnixSecondsToDateTime(vw_WindModel.timestamp_local).ToString($"MM/dd/yyyy {settings.TimeFormat}:mm:ss");
        wind_direction_short_cardinal = Constants.GetShortCardinalDirection(this._vw_WindModel.wind_direction);
        wind_speed = new Amount(vw_WindModel.wind_speed, VW_WindModel.WindSpeedUnit).ConvertedTo(settings.WindspeedUnit).Value;
    }

    public string timestamp_local_formatted { get; private set; }
    public double wind_direction => _vw_WindModel.wind_direction;
    public string wind_direction_short_cardinal { get; private set; }
    public double wind_speed { get; private set; }
}