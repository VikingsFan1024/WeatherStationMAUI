// using directives for precision in what specific classes are employed
using DateTime = System.DateTime;

namespace TempestMonitor.ViewModels.History;

public sealed class HourlyObservationSummary
{
    public required long HoursBack { get; set; }
    public long Hour => MaximumTimestamp.Hour; // Expose the hour of the maximum timestamp for convenience
    public required long Count { get; set; }
    public required long LightningStrikesDuringHour { get; set; }
    public required double MinimumTemperature { get; set; }
    public required double AverageTemperature { get; set; }
    public required double MaximumTemperature { get; set; }
    public required DateTime MinimumTimestamp { get; set; }
    public required DateTime MaximumTimestamp { get; set; }
    public required double AverageWindDirection { get; set; }
    public required string AverageWindDirectionShortCardinal { get; set; }
    public required double MaximumWindGust { get; set; }
    public required double AverageWindspeed { get; set; }
    public required double RainAccumulationDuringHour { get; set; }
    public double RainAccumulationTotal { get; set; }
    public required double AverageLightningStrikeDistance { get; set; }

    public required double MinimumRelativeHumidity { get; set; }
    public required double AverageRelativeHumidity { get; set; }
    public required double MaximumRelativeHumidity { get; set; }

    public required double MinimumBattery { get; set; }
    public required double AverageBattery { get; set; }
    public required double MaximumBattery { get; set; }

    public required double MinimumIlluminance { get; set; }
    public required double AverageIlluminance { get; set; }
    public required double MaximumIlluminance { get; set; }

    public required double MinimumUV { get; set; }
    public required double AverageUV { get; set; }
    public required double MaximumUV { get; set; }
    public required double MinimumSolarRadiation { get; set; }
    public required double AverageSolarRadiation { get; set; }
    public required double MaximumSolarRadiation { get; set; }
    public required double MinimumStationPressure { get; set; }
    public required double AverageStationPressure { get; set; }
    public required double MaximumStationPressure { get; set; }

}
