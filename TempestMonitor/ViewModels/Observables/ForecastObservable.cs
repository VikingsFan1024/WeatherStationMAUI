using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestMonitor.Models;

namespace TempestMonitor.ViewModels.Observables;

public partial class ForecastObservable : ObservableObject
{
    private readonly ForecastModel _forecast;
    private readonly CurrentConditionsObservable _currentConditions;
    private readonly ObservableCollection<HourlyObservable> _observableHourlies;
    private readonly ObservableCollection<DailyObservable> _observableDailies;
    private readonly StationObservable _stationObservable;
    private readonly UnitsObservable _unitsObservable;
    public ForecastObservable(ForecastModel forecast, SettingsModel settings) : base()
    {
        _forecast = forecast;
        _currentConditions = new CurrentConditionsObservable(forecast.CurrentConditions, settings);
        _observableHourlies = HourlyObservable.ConvertToObservableCollection(forecast.Hourlies, settings);
        _observableDailies = DailyObservable.ConvertToObservableCollection(forecast.Dailies, settings);
        _stationObservable = new StationObservable(forecast.Station, settings);
        _unitsObservable = new UnitsObservable(forecast.Units);
    }

    public DateTime Timestamp => Constants.UnixSecondsToLocalTime(_forecast.Timestamp);
    public double Latitude => Constants.LongToDouble(_forecast.Latitude);
    public string LocationName => _forecast.LocationName;
    public double Longitude => Constants.LongToDouble(_forecast.Longitude);
    public string Timezone => _forecast.Timezone;
    public long TimezoneOffsetMinutes => _forecast.TimezoneOffsetMinutes;
    public long SourceIdConditions => _forecast.SourceIdConditions;
    public CurrentConditionsObservable CurrentConditions => _currentConditions;
    public ObservableCollection<HourlyObservable> HourlyForecasts => _observableHourlies;
    public ObservableCollection<DailyObservable> DailyForecasts => _observableDailies;
    public StationObservable Station => _stationObservable;
    public UnitsObservable Units => _unitsObservable;
}
