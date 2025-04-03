using Serilog;
using RedStar.Amounts;
using RedStar.Amounts.StandardUnits;

using System.IO;
using System;
using Microsoft.Maui.Storage; // For Preferences

namespace TempestMonitor.Models;

public class SettingsModel
{
    public static readonly string[] BatteryUnits =
        [
            ElectricUnits.Volt.Name,
        ];

    public static readonly string[] DistanceUnitOptions =
        [
            LengthUnits.Mile.Name,
            LengthUnits.KiloMeter.Name,
        ];

    public static readonly string[] ElevationUnitOptions =
        [
            LengthUnits.Foot.Name,
            LengthUnits.Meter.Name,
        ];

    public static readonly string[] PrecipitationUnitOptions =
        [
            LengthUnits.Inch.Name,
            LengthUnits.CentiMeter.Name,
            LengthUnits.MilliMeter.Name,
        ];

    public static readonly string[] PressureUnitOptions =
        [
            PressureUnits.InchOfMercury.Name,
            PressureUnits.MilliBar.Name,
        ];

    public static readonly string[] TemperatureUnitOptions =
        [
            TemperatureUnits.DegreeFahrenheit.Name,
            TemperatureUnits.DegreeCelcius.Name,
        ];

    public static readonly string[] TimeFormatOptions = [@"hh", @"HH"];

    public static readonly string[] WindspeedUnitOptions =
        [
            SpeedUnits.MilePerHour.Name,
            SpeedUnits.KilometerPerHour.Name,
            SpeedUnits.MeterPerSecond.Name,
            SpeedUnits.Knot.Name,
        ];

    public string BatteryUnit { get; set; } = BatteryUnits[0];
    public string DistanceUnit { get; set; } = DistanceUnitOptions[0];
    public string ElevationUnit { get; set; } = ElevationUnitOptions[0];
    public string PrecipitationUnit { get; set; } = PrecipitationUnitOptions[0];
    public string PressureUnit { get; set; } = PressureUnitOptions[0];
    public string TemperatureUnit { get; set; } = TemperatureUnitOptions[0];
    public string TimeFormat { get; set; } = TimeFormatOptions[0];
    public string WindspeedUnit { get; set; } = WindspeedUnitOptions[0];

    public string BatteryUnitSymbol => UnitManager.GetUnitByName(BatteryUnit).Symbol;
    public string DistanceUnitSymbol => UnitManager.GetUnitByName(DistanceUnit).Symbol;
    public string ElevationUnitSymbol => UnitManager.GetUnitByName(ElevationUnit).Symbol;
    public string IlluminanceUnitSymbol => @"lux";
    public string PrecipitationUnitSymbol => UnitManager.GetUnitByName(PrecipitationUnit).Symbol;
    public string PressureUnitSymbol => UnitManager.GetUnitByName(PressureUnit).Symbol;
    public string SolarRaditionUnitSymbol => @"W/m²";
    public string TemperatureUnitSymbol => UnitManager.GetUnitByName(TemperatureUnit).Symbol;
    public string WindspeedUnitSymbol => UnitManager.GetUnitByName(WindspeedUnit).Symbol;

    public string DatabaseFilename => Path.Combine(DatabaseFolder, "TempestMonitor.db");
    public string DatabaseFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Databases");
    public string LogFilename => Path.Combine(LogFolder, "TempestMonitor.log");
    public string LogFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Logs");
    public string RestAPIKey { get; set; } = @"c83c3cb2-a6d2-4c6f-b2f4-56aa9ab7306e";
    public string StationID { get; set; } = @"160805";
    public long TimeBetweenHttpRequestsInMinutes { get; set; } = 15;
    public SettingsModel()
    {
        GetSettings();
    }
    public void SaveSettings()
    {
        SaveSetting(nameof(BatteryUnit), BatteryUnit);
        SaveSetting(nameof(DatabaseFolder), DatabaseFolder);
        SaveSetting(nameof(DistanceUnit), DistanceUnit);
        SaveSetting(nameof(LogFolder), LogFolder);
        SaveSetting(nameof(PrecipitationUnit), PrecipitationUnit);
        SaveSetting(nameof(PressureUnit), PressureUnit);
        SaveSetting(nameof(RestAPIKey), RestAPIKey);
        SaveSetting(nameof(StationID), StationID);
        SaveSetting(nameof(TemperatureUnit), TemperatureUnit);
        SaveSetting(nameof(TimeBetweenHttpRequestsInMinutes), TimeBetweenHttpRequestsInMinutes);
        SaveSetting(nameof(TimeFormat), TimeFormat);
        SaveSetting(nameof(WindspeedUnit), WindspeedUnit);
    }
    public static void SaveSetting(string key, object settingValue)
    {
        try
        {
            if (settingValue is string stringValue)
                Preferences.Default.Set(key, stringValue);
            else if (settingValue is long longValue)
                Preferences.Default.Set(key, longValue);
            else
                Log.Error($"Unknown setting type: {settingValue.GetType().Name}");
        }

        catch (Exception ex)
        {
            Log.Error(ex, $"Error saving setting {key}");
        }
    }
    public void GetSettings()
    {
        var IPreferences = Preferences.Default;
        BatteryUnit = IPreferences.Get(nameof(BatteryUnit),BatteryUnit);
        DatabaseFolder = IPreferences.Get(nameof(DatabaseFolder),DatabaseFolder);
        DistanceUnit = IPreferences.Get(nameof(DistanceUnit),DistanceUnit);
        LogFolder = IPreferences.Get(nameof(LogFolder),LogFolder);
        PrecipitationUnit = IPreferences.Get(nameof(PrecipitationUnit),PrecipitationUnit);
        PressureUnit = IPreferences.Get(nameof(PressureUnit),PressureUnit);
        RestAPIKey = IPreferences.Get(nameof(RestAPIKey),RestAPIKey);
        StationID = IPreferences.Get(nameof(StationID),StationID);
        TemperatureUnit = IPreferences.Get(nameof(TemperatureUnit),TemperatureUnit);
        TimeBetweenHttpRequestsInMinutes = IPreferences.Get(
            nameof(TimeBetweenHttpRequestsInMinutes),TimeBetweenHttpRequestsInMinutes);
        TimeFormat = IPreferences.Get(nameof(TimeFormat),TimeFormat);
        WindspeedUnit = IPreferences.Get(nameof(WindspeedUnit),WindspeedUnit);
    }
}