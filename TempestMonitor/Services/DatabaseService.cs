using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;
using static System.Linq.Enumerable; // For ToArray() by JsonElement.ArrayEnumerator

using DateTime = System.DateTime;
using DateTimeOffset = System.DateTimeOffset;
using Exception = System.Exception;
using IServiceProvider = System.IServiceProvider;
using Log = Serilog.Log;
using Mutex = System.Threading.Mutex;
using ObservationModel = TempestMonitor.Models.ObservationModel;
using SettingsModel = TempestMonitor.Models.SettingsModel; // For SettingsModel to get the database filename
using SQLiteConnection = SQLite.SQLiteConnection; // for SQLiteConnection, to avoid ambiguity with System.Data.SQLite

namespace TempestMonitor.Services;
public class DatabaseService(IServiceProvider serviceProvider)
{
    private readonly static Mutex _databaseConnectionMutex = new();
    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private static long GetCurrentLocalUnixSeconds()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }
    private static long GetMidnightLocalInUnixSeconds()
    {
        return new DateTimeOffset(DateTime.Now.Date).ToUnixTimeSeconds();
    }
    public ObservationModel[]? GetLast24HoursOfSavedObservations(
        long currentLocalTimeInUnixSeconds = 0)
    {
        if (currentLocalTimeInUnixSeconds == 0)
            currentLocalTimeInUnixSeconds = GetCurrentLocalUnixSeconds();

        var twentyFourLocalHoursBackUnixSeconds = currentLocalTimeInUnixSeconds - 86400; // 24 hours in seconds

        try
        {
            _databaseConnectionMutex.WaitOne();
            using var databaseConnection = new SQLiteConnection(_settings.DatabaseFilename);

            try
            {
                var last24HoursOfObservationModels = databaseConnection.Table<ObservationModel>()
                    .Where
                    (
                        x =>
                            x.ObservationTimestamp >= twentyFourLocalHoursBackUnixSeconds &&
                            x.ObservationTimestamp <= currentLocalTimeInUnixSeconds
                    )
                    .Select(x => x)
                    .OrderBy(x => x.ObservationTimestamp)
                    .ToArray();

                return last24HoursOfObservationModels;

                //    .GroupBy(x => x.Hour)

                //    .Select(g =>
                //        (
                //            new ObservationModel[]
                //            {
                //                new ObservationModel()
                //                {
                //                    AirTemperature = (long)Math.Round(g.Average(x => x.AirTemperature), 0, MidpointRounding.AwayFromZero),
                //                    Battery = (long)Math.Round(g.Average(x => x.Battery), 0, MidpointRounding.AwayFromZero),
                //                    FirmwareRevision = (long)Math.Round(g.Average(x => x.FirmwareRevision), 0, MidpointRounding.AwayFromZero),
                //                    Illuminance = (long)Math.Round(g.Average(x => x.Illuminance), 0, MidpointRounding.AwayFromZero),
                //                    LightningStrikeAverageDistance = (long)Math.Round(g.Average(x => x.LightningStrikeAverageDistance), 0, MidpointRounding.AwayFromZero),
                //                    LightningStrikeCount = (long)Math.Round(g.Average(x => x.LightningStrikeCount), 0, MidpointRounding.AwayFromZero),
                //                    ObservationTimestamp = (long)Math.Round(g.Average(x => x.ObservationTimestamp), 0, MidpointRounding.AwayFromZero),
                //                    PrecipitationType = (long)Math.Round(g.Average(x => x.PrecipitationType), 0, MidpointRounding.AwayFromZero),
                //                    RainAccumulationOverThePreviousMinute = (long)Math.Round(g.Average(x => x.RainAccumulationOverThePreviousMinute), 0, MidpointRounding.AwayFromZero),
                //                    RelativeHumidity = (long)Math.Round(g.Average(x => x.RelativeHumidity), 0, MidpointRounding.AwayFromZero),
                //                    SolarRadiation = (long)Math.Round(g.Average(x => x.SolarRadiation), 0, MidpointRounding.AwayFromZero),
                //                    StationPressure = (long)Math.Round(g.Average(x => x.StationPressure), 0, MidpointRounding.AwayFromZero),
                //                    UV = (long)Math.Round(g.Average(x => x.UV), 0, MidpointRounding.AwayFromZero),
                //                    WindAverage = (long)Math.Round(g.Average(x => x.WindAverage), 0, MidpointRounding.AwayFromZero),
                //                    WindDirection = (long)Math.Round(g.Average(x => x.WindDirection), 0, MidpointRounding.AwayFromZero),
                //                    WindGust = (long)Math.Round(g.Average(x => x.WindGust), 0, MidpointRounding.AwayFromZero),
                //                    WindLull = (long)Math.Round(g.Average(x => x.WindLull), 0, MidpointRounding.AwayFromZero),
                //                    WindSampleIntervalInMinutes = (long)Math.Round(g.Average(x => x.WindSampleIntervalInMinutes), 0, MidpointRounding.AwayFromZero),
                //                },
                //                new ObservationModel()
                //                {
                //                    AirTemperature = g.Max(x => x.AirTemperature),
                //                    Battery = g.Max(x => x.Battery),
                //                    FirmwareRevision = g.Max(x => x.FirmwareRevision),
                //                    Illuminance = g.Max(x => x.Illuminance),
                //                    LightningStrikeAverageDistance = g.Max(x => x.LightningStrikeAverageDistance),
                //                    LightningStrikeCount = g.Max(x => x.LightningStrikeCount),
                //                    ObservationTimestamp = g.Max(x => x.ObservationTimestamp),
                //                    PrecipitationType = g.Max(x => x.PrecipitationType),
                //                    RainAccumulationOverThePreviousMinute = g.Max(x => x.RainAccumulationOverThePreviousMinute),
                //                    RelativeHumidity = g.Max(x => x.RelativeHumidity),
                //                    SolarRadiation = g.Max(x => x.SolarRadiation),
                //                    StationPressure = g.Max(x => x.StationPressure),
                //                    UV = g.Max(x => x.UV),
                //                    WindAverage = g.Max(x => x.WindAverage),
                //                    WindDirection = g.Max(x => x.WindDirection),
                //                    WindGust = g.Max(x => x.WindGust),
                //                    WindLull = g.Max(x => x.WindLull),
                //                    WindSampleIntervalInMinutes = g.Max(x => x.WindSampleIntervalInMinutes),
                //                },
                //                new ObservationModel()
                //                {
                //                    AirTemperature = g.Min(x => x.AirTemperature),
                //                    Battery = g.Min(x => x.Battery),
                //                    FirmwareRevision = g.Min(x => x.FirmwareRevision),
                //                    Illuminance = g.Min(x => x.Illuminance),
                //                    LightningStrikeAverageDistance = g.Min(x => x.LightningStrikeAverageDistance),
                //                    LightningStrikeCount = g.Min(x => x.LightningStrikeCount),
                //                    ObservationTimestamp = g.Min(x => x.ObservationTimestamp),
                //                    PrecipitationType = g.Min(x => x.PrecipitationType),
                //                    RainAccumulationOverThePreviousMinute = g.Min(x => x.RainAccumulationOverThePreviousMinute),
                //                    RelativeHumidity = g.Min(x => x.RelativeHumidity),
                //                    SolarRadiation = g.Min(x => x.SolarRadiation),
                //                    StationPressure = g.Min(x => x.StationPressure),
                //                    UV = g.Min(x => x.UV),
                //                    WindAverage = g.Min(x => x.WindAverage),
                //                    WindDirection = g.Min(x => x.WindDirection),
                //                    WindGust = g.Min(x => x.WindGust),
                //                    WindLull = g.Min(x => x.WindLull),
                //                    WindSampleIntervalInMinutes = g.Min(x => x.WindSampleIntervalInMinutes),
                //                },
                //                new ObservationModel()
                //                {
                //                    AirTemperature = g.Sum(x => x.AirTemperature),
                //                    Battery = g.Sum(x => x.Battery),
                //                    FirmwareRevision = g.Sum(x => x.FirmwareRevision),
                //                    Illuminance = g.Sum(x => x.Illuminance),
                //                    LightningStrikeAverageDistance = g.Sum(x => x.LightningStrikeAverageDistance),
                //                    LightningStrikeCount = g.Sum(x => x.LightningStrikeCount),
                //                    ObservationTimestamp = g.Sum(x => x.ObservationTimestamp),
                //                    PrecipitationType = g.Sum(x => x.PrecipitationType),
                //                    RainAccumulationOverThePreviousMinute = g.Sum(x => x.RainAccumulationOverThePreviousMinute),
                //                    RelativeHumidity = g.Sum(x => x.RelativeHumidity),
                //                    SolarRadiation = g.Sum(x => x.SolarRadiation),
                //                    StationPressure = g.Sum(x => x.StationPressure),
                //                    UV = g.Sum(x => x.UV),
                //                    WindAverage = g.Sum(x => x.WindAverage),
                //                    WindDirection = g.Sum(x => x.WindDirection),
                //                    WindGust = g.Sum(x => x.WindGust),
                //                    WindLull = g.Sum(x => x.WindLull),
                //                    WindSampleIntervalInMinutes = g.Sum(x => x.WindSampleIntervalInMinutes),
                //                }
                //            }
                //        )
                //    )
                //    .OrderBy(z => z[0].ObservationTimestamp)
                //    .Select(x => x)
                //    .ToArray();

                //return last24HourAverageMaximumMinimumSumObservationModel;
            }

            // No result generates InvalidOperationException
            catch (System.InvalidOperationException invalidOperationException)
            {
                Log.Error(invalidOperationException, "Exception no results");
                return null;
            }

            catch (Exception exception)
            {
                Log.Error(exception, "Exception");
                throw;
            }
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception");
            throw;
        }

        finally
        {
            _databaseConnectionMutex.ReleaseMutex();
        }
    }
    public bool SaveBufferToDB(object classInstance)
    {
        var result = false;

        _databaseConnectionMutex.WaitOne();
        using var databaseConnection = new SQLiteConnection(_settings.DatabaseFilename);

        try
        {
            result = databaseConnection?.Insert(classInstance) == 1;
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception");
            throw;
        }

        finally
        {
            _databaseConnectionMutex.ReleaseMutex();
        }

        if (!result) Log.Error("SaveBufferToDB not 1 row inserted");

        return result;
    }
}
