namespace TempestMonitor;

public class MonitorSQLiteConnection : SQLiteConnection
{
    public MonitorSQLiteConnection(string databasePath) : base(databasePath)
    {
//        TableChanged += OnTableChanged;
    }

    //private void OnTableChanged(object? sender, SQLite.NotifyTableChangedEventArgs e)
    //{
    //    if (e.Action == SQLite.NotifyTableChangedAction.Insert)
    //    {
    //        WeakReferenceMessenger.Default.Send
    //        (
                
    //            new VW_Message<InsertedObjectEssence>
    //            (
    //                new InsertedObjectEssence() { TableName = e.Table.TableName, RowId = SQLite3.LastInsertRowid(Handle), 
    //                    JsonDocument = e.Table.FindColumn("jsonDocument").GetValue(Handle, e.RowId).ToString() ?? string.Empty
    //                }
    //            )
    //        );
    //    }

    //}
}
