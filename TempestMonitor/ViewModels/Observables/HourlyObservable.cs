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

public partial class HourlyObservable : BaseObservable
{
    private readonly HourlyModel _hourly;

    public HourlyObservable(HourlyModel hourly, int rowNumber, SettingsModel settings) : base(settings)
    {
        _hourly = hourly;
        RowNumber = rowNumber;
        AirTemperature = new Amount(
            Constants.LongToDouble(_hourly.AirTemperature),
            _hourly.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        FeelsLike = new Amount(
            Constants.LongToDouble(_hourly.FeelsLike),
            _hourly.Units.TemperatureUnit
        ).ConvertedTo(_settings.TemperatureUnit).Value;
        Precipitation = new Amount(
            Constants.LongToDouble(_hourly.Precipitation),
            _hourly.Units.PrecipitationUnit
        ).ConvertedTo(_settings.PrecipitationUnit).Value;
        SeaLevelPressure = new Amount(
            Constants.LongToDouble(_hourly.SeaLevelPressure),
            _hourly.Units.PressureUnit
        ).ConvertedTo(_settings.PressureUnit).Value;
        StationPressure = new Amount(
            Constants.LongToDouble(_hourly.StationPressure),
            _hourly.Units.PressureUnit
        ).ConvertedTo(_settings.PressureUnit).Value;
        WindAvg = new Amount(
            Constants.LongToDouble(_hourly.WindAvg),
            _hourly.Units.WindspeedUnit
        ).ConvertedTo(_settings.WindspeedUnit).Value;
    }
    public int RowNumber { get; private set; }
    public double AirTemperature { get; private set; }
    public string Conditions => _hourly.Conditions;
    public double FeelsLike { get; private set; }
    public DateTime Time => Constants.UnixSecondsToLocalTime(_hourly.Time);
    public double Precipitation { get; private set; }
    public double PrecipitationProbability => Constants.LongToDouble(_hourly.PrecipitationProbability);
    public double RelativeHumidity => Constants.LongToDouble(_hourly.RelativeHumidity);
    public double SeaLevelPressure { get; private set; }
    public double StationPressure { get; private set; }
    public double UV => Constants.LongToDouble(_hourly.UV);
    public double WindAvg { get; private set; }
    public double WindDirection => _hourly.WindDirection;
    public long LocalDay => _hourly.LocalDay;
    public long LocalHour => _hourly.LocalHour;
    public string Icon => _hourly.Icon;
    public string PrecipitationIcon => _hourly.PrecipitationIcon;
    public string PrecipitationType => _hourly.PrecipitationType;
    public string WindDirectionCardinal => _hourly.WindDirectionCardinal;
    public static ObservableCollection<HourlyObservable> ConvertToObservableCollection(
        HourlyModel[] hourlies, SettingsModel settings, int takeCount = 24)
    {
        int rowNumber = 1;
        return new ObservableCollection<HourlyObservable>(
            hourlies.Select(hourly => new HourlyObservable(hourly, rowNumber++, settings))
        );
    }
}