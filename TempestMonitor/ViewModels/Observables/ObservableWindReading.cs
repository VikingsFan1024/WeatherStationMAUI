﻿namespace TempestMonitor.ViewModels.Observables;

public partial class ObservableWindReading : ObservableObject
{
    public WindReadingModel WindReading { get; private set;}
    public ObservableWindReading(WindReadingModel windReading, SettingsModel settings)
    {
        HubSN = windReading.HubSN;
        WindDirectionDegrees = Constants.LongToDouble(windReading.WindDirection);
        WindDirectionShortCardinal = Constants.GetShortCardinalDirection(windReading.WindDirection);
        WindReading = windReading;
        WindTimestampTimeOnlyString = Constants.UnixSecondsToLocalTime(windReading.WindTimestamp).ToString(settings.TimeFormat + ":mm:ss", CultureInfo.InvariantCulture);
        Windspeed = new Amount(
            Constants.LongToDouble(WindReading.Windspeed)
            ,WindReadingModel.PropertyUnit[nameof(WindReadingModel.Windspeed)]
        ).ConvertedTo(settings.WindspeedUnit).Value;
    }

    public string HubSN { get; private set; }
    public string WindTimestampTimeOnlyString { get; private set; }
    public double Windspeed { get; private set; }
    public string WindDirectionShortCardinal { get; private set; }
    public double WindDirectionDegrees { get; private set; }
}