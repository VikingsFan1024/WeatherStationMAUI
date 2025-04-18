﻿namespace TempestMonitor.ViewModels;

sealed partial class HistoryViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private readonly DatabaseService _databaseService = serviceProvider.GetRequiredService<DatabaseService>();
    private ObservableCollectionOfHourlyObservationSummary _hourlyObservationSummaries = [];

    public void OnAppearing()
    {
        long startTime = 0; 
        // DateTimeOffset.Now.ToUnixTimeSeconds() - (86400 * 2);
        var last24HoursOfObservationModels = _databaseService.GetLast24HoursOfSavedObservations(startTime);

        //if (last24HoursOfObservationModels is not null)
        //{
        //    using StreamWriter writetext = new StreamWriter(@"C:\Temp\data.csv");

        //    foreach (var obs in last24HoursOfObservationModels)
        //    {
        //        if (obs is not null)
        //        {
        //            writetext.WriteLine($"{Constants.UnixSecondsToLocalTime(obs.ObservationTimestamp)},{obs.RainAccumulationOverThePreviousMinute}");
        //        }
        //    }
        //}
        //DateTimeOffset.Now.ToUnixTimeSeconds() - (86400*2));

        if (last24HoursOfObservationModels is null || last24HoursOfObservationModels.Length == 0) return;

        var midnightTodayUnixSeconds = new DateTimeOffset(DateTime.Now.Date).ToUnixTimeSeconds();
        var currentHourSecondsPastMidnight = DateTime.Now.Hour * 3600;
        var currentTopOfHourUnixSeconds = midnightTodayUnixSeconds + currentHourSecondsPastMidnight;

        try
        {
            _hourlyObservationSummaries = new
            (
                last24HoursOfObservationModels
                .GroupBy(x => (currentTopOfHourUnixSeconds - x.ObservationTimestamp) / 3600)
                .Select
                (
                    (y, i) => new HourlyObservationSummary
                    {
                        HoursBack = y.Key,
                        Count = y.Count(),

                        MinimumTimestamp = ObservableObservation.RawToUIObservationTimestamp(
                            y.Min(y => y.ObservationTimestamp), _settings),
                        MaximumTimestamp = ObservableObservation.RawToUIObservationTimestamp(
                            y.Max(y => y.ObservationTimestamp), _settings),

                        MinimumTemperature = ObservableObservation.RawToUIAirTemperature(
                            y.Min(y => y.AirTemperature), _settings),
                        AverageTemperature = ObservableObservation.RawToUIAirTemperature(
                            (long)(double.Round(y.Average(y => y.AirTemperature), 0)), _settings),
                        MaximumTemperature = ObservableObservation.RawToUIAirTemperature(
                            y.Max(y => y.AirTemperature), _settings),

                        AverageWindspeed = ObservableObservation.RawToUIWind(
                            (long)(double.Round(y.Average(y => y.WindAverage), 0)), _settings),
                        MaximumWindGust = ObservableObservation.RawToUIWind(y.Max(y => y.WindGust), _settings),

                        AverageWindDirection = ObservableObservation.RawToUIWindDirection(
                            (long)(double.Round(y.Average(y => y.WindDirection), 0)), _settings),
                        AverageWindDirectionShortCardinal = ObservableObservation.RawToUIWindDirectionShortCardinal(
                            (long)(double.Round(y.Average(y => y.WindDirection), 0)), _settings),

                        RainAccumulationDuringHour = ObservableObservation.RawToUIRainAccumulationOverThePreviousMinute(
                            y.Sum(y => y.RainAccumulationOverThePreviousMinute), _settings),
                        LightningStrikesDuringHour = ObservableObservation.RawToUILightningStrikeCount(
                            y.Sum(y => y.LightningStrikeCount), _settings),
                        AverageLightningStrikeDistance = ObservableObservation.RawToUILightningStrikeAverageDistance(
                            (long)(double.Round(y.Average(y => y.LightningStrikeAverageDistance), 0)), _settings),

                        MinimumRelativeHumidity = ObservableObservation.RawToUIRelativeHumidity(
                            y.Min(y => y.RelativeHumidity), _settings),
                        AverageRelativeHumidity = ObservableObservation.RawToUIRelativeHumidity(
                            (long)(double.Round(y.Average(y => y.RelativeHumidity), 0)), _settings),
                        MaximumRelativeHumidity = ObservableObservation.RawToUIRelativeHumidity(
                            y.Max(y => y.RelativeHumidity), _settings),

                        MinimumBattery = ObservableObservation.RawToUIBattery(y.Min(y => y.Battery), _settings),
                        AverageBattery = ObservableObservation.RawToUIBattery(
                            (long)(double.Round(y.Average(y => y.Battery), 0)), _settings),
                        MaximumBattery = ObservableObservation.RawToUIBattery(y.Max(y => y.Battery), _settings),

                        MinimumIlluminance = ObservableObservation.RawToUIIlluminance(
                            y.Min(y => y.Illuminance), _settings),
                        AverageIlluminance = ObservableObservation.RawToUIIlluminance(
                            (long)(double.Round(y.Average(y => y.Illuminance), 0)), _settings),
                        MaximumIlluminance = ObservableObservation.RawToUIIlluminance(
                            y.Max(y => y.Illuminance), _settings),
                        MinimumUV = ObservableObservation.RawToUIUV(y.Min(y => y.UV), _settings),
                        AverageUV = ObservableObservation.RawToUIUV(
                            (long)(double.Round(y.Average(y => y.UV), 0)), _settings),
                        MaximumUV = ObservableObservation.RawToUIUV(y.Max(y => y.UV), _settings),
                        MinimumSolarRadiation = ObservableObservation.RawToUISolarRadiation(
                            y.Min(y => y.SolarRadiation), _settings),
                        AverageSolarRadiation = ObservableObservation.RawToUISolarRadiation(
                            (long)(double.Round(y.Average(y => y.SolarRadiation), 0)), _settings),
                        MaximumSolarRadiation = ObservableObservation.RawToUISolarRadiation(
                            y.Max(y => y.SolarRadiation), _settings),
                        MinimumStationPressure = ObservableObservation.RawToUIStationPressure(
                            y.Min(y => y.StationPressure), _settings),
                        AverageStationPressure = ObservableObservation.RawToUIStationPressure(
                            (long)(double.Round(y.Average(y => y.StationPressure), 0)), _settings),
                        MaximumStationPressure = ObservableObservation.RawToUIStationPressure(
                            y.Max(y => y.StationPressure), _settings),
                    }
                )
                .OrderByDescending
                (
                    // Order by the hour of the day (0-23) for the summary, then by the minimum timestamp for consistency
                    x => x.HoursBack
                )
                .ToList()
            );

            if (_hourlyObservationSummaries.Any())
            {
                var first = _hourlyObservationSummaries.First(); // Get the first summary
                first.RainAccumulationTotal = first.RainAccumulationDuringHour; // Initialize the total with the first summary's value

                var result = _hourlyObservationSummaries
                    .Select((summary, index) =>
                    {
                        if (index == 0) return summary;
                        // The first summary already has the initialized total
                        // For subsequent summaries, accumulate the rain total from the previous summary
                        summary.RainAccumulationTotal = _hourlyObservationSummaries[index - 1].RainAccumulationTotal +
                                                       summary.RainAccumulationDuringHour;
                        return summary;
                    }).ToList();
            }
        }

        catch (Exception ex)
        {
            Log.Error(ex, "Error processing observations: {Message}", ex.Message);
            throw;
        }

        OnPropertyChanged(nameof(HourlyObservationSummaries));
        OnPropertyChanged(nameof(Settings));
    }

    public ObservableCollectionOfHourlyObservationSummary HourlyObservationSummaries => _hourlyObservationSummaries;
    public SettingsModel Settings => _settings;
}