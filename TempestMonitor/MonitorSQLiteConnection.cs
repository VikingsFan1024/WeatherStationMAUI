using TempestMonitor.Models;

namespace TempestMonitor;

public class MonitorSQLiteConnection : SQLiteConnection
{
    public MonitorSQLiteConnection(string databasePath) : base(databasePath)
    {
        TableChanged += OnTableChanged;
    }

    private void OnTableChanged(object? sender, SQLite.NotifyTableChangedEventArgs e)
    {
        if (e.Action == SQLite.NotifyTableChangedAction.Insert)
        {
            WeakReferenceMessenger.Default.Send
            (
                new DatabaseTableAndKeyMessage
                (
                    new TablenameRowId() { TableName = e.Table.TableName, RowId = SQLite3.LastInsertRowid(Handle) }
                )
            );
        }

    }
}
