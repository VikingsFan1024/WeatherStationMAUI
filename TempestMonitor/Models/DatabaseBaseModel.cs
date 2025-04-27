namespace TempestMonitor.Models;

public abstract class DatabaseBaseModel
{
    [Column("Id"), PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }
    [Column("json_document")]
    public byte[] json_document { get; set; } = [];
}

public class TablenameRowId
{
    public string TableName { get; set; } = string.Empty;
    public long RowId { get; set; }
}
public class DatabaseTableAndKeyMessage(TablenameRowId tablenameRowId) : ValueChangedMessageOfTablenameRowId(tablenameRowId)
{
    private readonly TablenameRowId _tablenameRowId = tablenameRowId;
    public TablenameRowId TablenameRowId => _tablenameRowId;
}
