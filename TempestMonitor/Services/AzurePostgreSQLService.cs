using Npgsql;
using NpgsqlTypes;
using Exception = System.Exception;          // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
using StringBuilder = System.Text.StringBuilder;
using TableAndReadingTypeToDataAssociation = TempestMonitor.Models.TableAndReadingTypeToDataAssociation;
using Task = System.Threading.Tasks.Task;    // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
namespace TempestMonitor.Services;
public class AzurePostgreSQLService(IServiceProvider serviceProvider)
{
    private readonly static Mutex _databaseConnectionMutex = new();
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isRunning;
    private ListOfTasks? _completionList;
    public void Start()
    {
        if (_isRunning)
        {
            Log.Information("Already running");
            return;
        }

        _cancellationTokenSource?.Dispose();
        _completionList?.Clear();

        _cancellationTokenSource = new();
        _completionList = [];

        Task.Run(() => Init());

        Log.Information("Started");
    }
    private bool Init()
    {
        if (_cancellationTokenSource is null) return false;
        if (_completionList is null) return false;

        try
        {
            _completionList.Add(Task.Run(() => HandleWeakReferenceMessages(), _cancellationTokenSource.Token));

            _isRunning = true;

            _cancellationTokenSource.Token.WaitHandle.WaitOne();

            return true;
        }

        catch (Exception exception)
        {
            Log.Error(exception, "Exception");
            return false;
        }
    }

    private void HandleWeakReferenceMessages()
    {
        WeakReferenceMessenger.Default.Register<VW_Message<TableAndReadingTypeToDataAssociation>>
        (
            this, (r, m) =>
            {
                try
                {
                    var tableAndReadingTypeToDataAssociation = m.Model;
                    if (tableAndReadingTypeToDataAssociation is null) return;

                    if (tableAndReadingTypeToDataAssociation.TableName is null) return;

                    SaveIt(tableAndReadingTypeToDataAssociation);
                    //databaseService.SaveBufferToDB(classInstance);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Exception");
                }
            }
        );

    }

    private void SaveIt(Models.TableAndReadingTypeToDataAssociation tableAndReadingTypeToDataAssociation)
    {
        var connString = "Server=azure-database-for-postgresql-flexible-server.postgres.database.azure.com;Database=weather_station_maui;Port=5432;User Id=dan;Password=Sylvia3927;Ssl Mode=Require;Pooling=false;";

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
        var dataSource = dataSourceBuilder.Build();

        try
        {
            _databaseConnectionMutex.WaitOne();
            using var connection = dataSource.OpenConnection();

            var snakeCaseTableName = ConvertToSnakeCase(tableAndReadingTypeToDataAssociation.TableName);

            if (tableAndReadingTypeToDataAssociation?.DataTypeInstance?.json_document is null) return;

            using (var cmd = new NpgsqlCommand($"INSERT INTO {snakeCaseTableName} (json_document_original) VALUES (@p)", connection))
            {
                cmd.Parameters.Add(new NpgsqlParameter("@p", NpgsqlDbType.Json) { Value = tableAndReadingTypeToDataAssociation.DataTypeInstance.json_document });
                cmd.ExecuteNonQuery();
            }
        }

        catch(NpgsqlException npgsqlException)
        {
            Log.Error(npgsqlException, "NpgsqlException");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Exception");
        }
        finally
        {
            _databaseConnectionMutex.ReleaseMutex();
        }
    }

    private static string ConvertToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new StringBuilder();

        foreach (char c in input)
        {
            if (char.IsUpper(c) && result.Length > 0)
                result.Append('_'); // Insert underscore before uppercase letters

            result.Append(char.ToLower(c)); // Convert to lowercase
        }

        return result.ToString();
    }

    public void Stop()
    {
        if (!_isRunning)
        {
            Log.Information("Not running");
            return;
        }

        if (_cancellationTokenSource is null) Log.Information("cancellationTokenSource is null");

        _cancellationTokenSource?.Cancel();

        WeakReferenceMessenger.Default.UnregisterAll(this);

        if (_completionList is not null)
        {
            foreach (var task in _completionList)
            {
                try
                {
                    if (!task.IsCanceled) task.Wait();
                }

                catch (AggregateException aggregateException)
                {
                    if (aggregateException.InnerException is TaskCanceledException)
                    {
                        Log.Information("TaskCanceledException in AggregateException, ignoring and continuing stop");
                        continue;
                    }
                    else
                    {
                        Log.Error(aggregateException, "Unexpected AggregateException");
                        break;
                    }
                }

                catch (Exception exception)
                {
                    Log.Error(exception, "Unrecognized exception");
                    break;
                }
            }
        }

        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = null;

        _isRunning = false;

        Log.Information("Stopped");
    }
}