namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableVW_WindModel : ObservableObject
{
    public VW_WindModel VW_WindModel { get; private set; }

    public ObservableVW_WindModel(VW_WindModel vw_WindModel, SettingsModel settings)
    {
        VW_WindModel = vw_WindModel;
        wind_direction_short_cardinal = Constants.GetShortCardinalDirection(VW_WindModel.wind_direction);
        wind_speed = new Amount(vw_WindModel.wind_speed, VW_WindModel.WindSpeedUnit)
            .ConvertedTo(settings.WindspeedUnit).Value;
    }

    public string hub_sn => VW_WindModel.hub_sn;
    public double wind_speed { get; private set; }
    public string wind_direction_short_cardinal { get; private set; }
    public double wind_direction => VW_WindModel.wind_direction;
}