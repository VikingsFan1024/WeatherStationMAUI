// using directives for precision in what specific classes are employed
using Amount = RedStar.Amounts.Amount;
using CultureInfo = System.Globalization.CultureInfo;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;
using SettingsModel = TempestMonitor.Models.SettingsModel;
using WindReadingModel = TempestMonitor.Models.WindReadingModel;

namespace TempestMonitor.ViewModels.Observables;

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