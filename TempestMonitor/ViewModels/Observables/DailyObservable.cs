using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TempestMonitor.Models;
using RedStar.Amounts;

namespace TempestMonitor.ViewModels.Observables;
public partial class DailyObservable : BaseObservable
{
    private readonly DailyModel _daily;

    public DailyObservable(DailyModel daily, int rowNumber, SettingsModel settings) : base(settings)
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
    public static ObservableCollection<DailyObservable> ConvertToObservableCollection(
        DailyModel[] dailies, SettingsModel settings)
    {
        int rowNumber = 1;
        return new ObservableCollection<DailyObservable>(
            dailies.Select(daily => new DailyObservable(daily, rowNumber++, settings))
        );
    }
}

