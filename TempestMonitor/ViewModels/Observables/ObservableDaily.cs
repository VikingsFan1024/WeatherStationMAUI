namespace TempestMonitor.ViewModels.Observables;
public partial class ObservableDaily : ObervableBase
{
    private readonly Daily _daily;

    public ObservableDaily(TempestRedStarMapping tempestRedStarMapping, Daily daily, int rowNumber, SettingsModel settings) : base(settings)
    {
        _daily = daily;
        RowNumber = rowNumber;
        air_temp_high = new Amount(_daily.air_temp_high, tempestRedStarMapping.units_temp)
            .ConvertedTo(_settings.TemperatureUnit).Value;

        air_temp_low = new Amount(_daily.air_temp_low, tempestRedStarMapping.units_temp)
            .ConvertedTo(_settings.TemperatureUnit).Value;

        sunset = Constants.UnixSecondsToDateTime(_daily.sunset);
        sunrise = Constants.UnixSecondsToDateTime(_daily.sunrise);
    }
    public int RowNumber { get; private set; }

    public double air_temp_high { get; private set; }
    public double air_temp_low { get; private set; }
    public string conditions => _daily.conditions;
    public double day_num => _daily.day_num;
    public DateTime day_start_local => Constants.UnixSecondsToDateTime(_daily.day_start_local);
    public string icon => _daily.icon;
    public double month_num => _daily.month_num;
    public string? precip_icon => _daily.precip_icon;
    public double? precip_probability => _daily.precip_probability;
    public string? precip_type => _daily.precip_type;
    public DateTime? sunrise { get; private set; }
    public DateTime? sunset { get; private set; }
    public static ObservableCollectionOfObservableDaily ConvertToObservableCollection(
        TempestRedStarMapping tempestRedStarMapping, Daily[] dailies, SettingsModel settings)
    {
        int rowNumber = 1;

        return new ObservableCollectionOfObservableDaily(
            dailies.Select(daily => new ObservableDaily(tempestRedStarMapping, daily, rowNumber++, settings))
        );
    }
}

