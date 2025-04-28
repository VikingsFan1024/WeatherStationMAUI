using Exception = System.Exception; // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
namespace TempestMonitor.Services;
public class DatabaseService
{
    private readonly static Mutex _databaseConnectionMutex = new();
    private readonly SettingsModel _settings;

    public DatabaseService(IServiceProvider serviceProvider)
    {
        _settings = serviceProvider.GetRequiredService<SettingsModel>();
        EnsureReadyForProcessing();
    }

    private readonly static string[] _tableNames =
    [
        "DeviceStatus",
        "HubStatus",
        "LightningStrike",
        "Observation",
        "RainStart",
        "WeatherForecast",
        "Wind",
        "VW_DeviceStatus",
        "VW_HourlyObservationSummary",
        "VW_HubStatus",
        "VW_LightningStrike",
        "VW_Observation",
        "VW_RainStart",
        "VW_Wind",
    ];
    public bool EnsureReadyForProcessing()
    {
        try
        {
            using var databaseConnection = new MonitorSQLiteConnection(_settings.DatabaseFilename);

            foreach (var tableName in _tableNames)
            {
                var filename = $"SQLQueries\\Tables\\{tableName}.sql";
                var stream = FileSystem.OpenAppPackageFileAsync(filename).GetAwaiter().GetResult();
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    var contents = reader.ReadToEnd();
                    reader.Close();

                    try
                    {
                        databaseConnection.Execute(contents);
                    }

                    catch (Exception exception)
                    {
                        if (!exception.Message.Contains("already exists"))
                        {
                            Log.Information(exception, $"Exception creating {tableName}");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        catch (Exception exception)
        {
            // ToDo: If we get here we can't continue so display error and exit app
            Log.Error(exception, "Exception");
            return false;
        }
    }
    private static long GetCurrentLocalUnixSeconds()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }
    private static long GetMidnightLocalInUnixSeconds()
    {
        return new DateTimeOffset(DateTime.Now.Date).ToUnixTimeSeconds();
    }

    public VW_HourlyObservationSummary[]? GetObservationSummary(int hoursBack = 72)
    {
        try
        {
            _databaseConnectionMutex.WaitOne();
            using var databaseConnection = new SQLiteConnection(_settings.DatabaseFilename);

            try
            {
                var results = databaseConnection.Query<VW_HourlyObservationSummary>(
                    "select * from VW_HourlyObservationSummary where hours_back < ? order by hours_back",
                    hoursBack + 1).ToArray();

                return results;
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
    //    public ObservationModel[]? GetLast24HoursOfSavedObservations(
    //        long currentLocalTimeInUnixSeconds = 0)
    //    {
    //        if (currentLocalTimeInUnixSeconds == 0)
    //            currentLocalTimeInUnixSeconds = GetCurrentLocalUnixSeconds();

    //        var twentyFourLocalHoursBackUnixSeconds = currentLocalTimeInUnixSeconds - 86400; // 24 hours in seconds

    //        try
    //        {
    //            _databaseConnectionMutex.WaitOne();
    //            using var databaseConnection = new SQLiteConnection(_settings.DatabaseFilename);

    //            try
    //            {
    //                var last24HoursOfObservationModels = databaseConnection.Table<ObservationModel>()
    //                    .Where
    //                    (
    //                        x =>
    //                            x.ObservationTimestamp >= twentyFourLocalHoursBackUnixSeconds &&
    //                            x.ObservationTimestamp <= currentLocalTimeInUnixSeconds
    //                    )
    //                    .Select(x => x)
    //                    .OrderBy(x => x.ObservationTimestamp)
    //                    .ToArray();

    //                return last24HoursOfObservationModels;
    //            }

    //            // No result generates InvalidOperationException
    //            catch (System.InvalidOperationException invalidOperationException)
    //            {
    //                Log.Error(invalidOperationException, "Exception no results");
    //                return null;
    //            }

    //            catch (Exception exception)
    //            {
    //                Log.Error(exception, "Exception");
    //                throw;
    //            }
    //        }

    //        catch (Exception exception)
    //        {
    //            Log.Error(exception, "Exception");
    //            throw;
    //        }

    //        finally
    //        {
    //            _databaseConnectionMutex.ReleaseMutex();
    //        }
    //    }

    public ObservationModel[]? GetObservationModels(long skipCount = 0, long fetchCount = 0)
    {
        try
        {
            _databaseConnectionMutex.WaitOne();
            using var databaseConnection = new SQLiteConnection(_settings.DatabaseFilename);
            try
            {
                var observationModels = databaseConnection.Table<ObservationModel>()
                    .Skip((int)skipCount)
                    .Take((int)fetchCount)
                    .ToArray();
                return observationModels;
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
        using var databaseConnection = new MonitorSQLiteConnection(_settings.DatabaseFilename);

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

    public T GetReadingByRowId<T>(long rowId) where T : DatabaseBaseModel, new()
    {
        try
        {
            _databaseConnectionMutex.WaitOne();
            using var databaseConnection = new MonitorSQLiteConnection(_settings.DatabaseFilename);
            try
            {
                var dataRow = databaseConnection.Table<T>()
                    .Where(x => x.Id == rowId)
                    .First();
                return dataRow;
            }
            // No result generates InvalidOperationException
            catch (System.InvalidOperationException invalidOperationException)
            {
                Log.Error(invalidOperationException, "Exception no results");
                throw;
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
}
