using Exception = System.Exception;          // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
using Task = System.Threading.Tasks.Task;    // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
namespace TempestMonitor.Services;
public class SQLiteDBService(IServiceProvider serviceProvider)
{
    private DatabaseService databaseService = serviceProvider.GetRequiredService<DatabaseService>();
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
        WeakReferenceMessenger.Default.Register<VW_Message<TempestMonitor.Models.TableAndReadingTypeToDataAssociation>>
        (
            this, (r, m) =>
            {
                try
                {
                    var classInstance = m.Model.DataTypeInstance;
                    if (classInstance is null)
                    {
                        Log.Error("classInstance is null");
                        return;
                    }
                    databaseService.SaveBufferToDB(classInstance);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Exception");
                }
            }
        );

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