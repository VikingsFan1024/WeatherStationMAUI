// static using for extension method classes
using static System.Linq.Enumerable;  // for Select LINQ method

// Aliases for types used in this file to keep the code cleaner
using ObservableCollectionOfObservableDaily = System.Collections.ObjectModel.ObservableCollection<TempestMonitor.ViewModels.Observables.ObservableDaily>;

// using directives for precision in what specific classes are employed
using Amount = RedStar.Amounts.Amount;
using DailyModel = TempestMonitor.Models.DailyModel;
using DateTime = System.DateTime;
using SettingsModel = TempestMonitor.Models.SettingsModel;

namespace TempestMonitor.ViewModels.Observables;
public partial class ObservableDaily : ObervableBase
{
    private readonly DailyModel _daily;

    public ObservableDaily(DailyModel daily, int rowNumber, SettingsModel settings) : base(settings)
    {
        _daily = daily;
        RowNumber = rowNumber;
        AirTempHigh = new Amount(
            Constants.LongToDouble(_daily.AirTempHigh),
            _daily.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        AirTempLow = new Amount(
            Constants.LongToDouble(_daily.AirTempLow),
            _daily.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
    }
    public int RowNumber { get; private set; }

    public double AirTempHigh { get; private set; }
    public double AirTempLow { get; private set; }
    public string Conditions => _daily.Conditions;
    public double DayNum => _daily.DayNumber;
    public DateTime DayStartLocal => Constants.UnixSecondsToLocalTime(_daily.DayStartLocal);
    public string Icon => _daily.Icon;
    public double MonthNumber => _daily.MonthNumber;
    public string? PrecipitationIcon => _daily.PrecipitationIcon;
    public double? PrecipitationProbability => Constants.LongToDouble(_daily.PrecipitationProbability);
    public string? PrecipitationType => _daily.PrecipitationType;
    public DateTime? Sunrise => Constants.UnixSecondsToLocalTime(_daily.Sunrise);
    public DateTime? Sunset => Constants.UnixSecondsToLocalTime(_daily.Sunset);
    public static ObservableCollectionOfObservableDaily ConvertToObservableCollection(
        DailyModel[] dailies, SettingsModel settings)
    {
        //ToDo: Change to use linq to create the collection
        int rowNumber = 1;
        return new ObservableCollectionOfObservableDaily(
            dailies.Select(daily => new ObservableDaily(daily, rowNumber++, settings))
        );
    }
}

