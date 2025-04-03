using System.Collections.Generic;

using ListOfStrings = System.Collections.Generic.List<string>;
using DateTimeOffset = System.DateTimeOffset;
using Guid = System.Guid;
using SQLite;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;
using NotSupportedException = System.NotSupportedException;

namespace TempestMonitor.Models;

public class ReadingModel
{
    public static readonly ListOfStrings supportedReadingTypes =
    [
        AirObservationModel.TypeName,
        DeviceStatusModel.TypeName,
        HubStatusModel.TypeName,
        LightningStrikeModel.TypeName,
        ObservationModel.TypeName,
        RainStartModel.TypeName,
        SkyObservationModel.TypeName,
        WindReadingModel.TypeName
    ];

    [Column("Id"), PrimaryKey]
    public string Id { get; set; } = string.Empty;
    [Column("JsonElementString")]
    public string JsonElementString { get; set; } = string.Empty;
    [Column("serial_number")]
    public string SerialNumber { get; set; } = string.Empty;
    [Column("Timestamp")]
    public long Timestamp { get; set; }
    [Column("type")]
    public string Type { get; set; } = string.Empty;
    [Ignore]
    [JsonIgnore]
    public JsonElement JsonElement { get; set; }
    public ReadingModel()
    {
    }
    public ReadingModel(ReadingModel readingModel)
    {
        Id = readingModel.Id;
        JsonElement = readingModel.JsonElement;
        JsonElementString = readingModel.JsonElementString;
        Type = readingModel.Type;
        SerialNumber = readingModel.SerialNumber;
        Timestamp = readingModel.Timestamp;
    }
    public ReadingModel(JsonElement jsonElement)
    {
        Id = Guid.NewGuid().ToString();
        JsonElement = jsonElement;
        JsonElementString = jsonElement.GetRawText();
        Type = jsonElement.GetProperty(@"type").ToString() ?? string.Empty;
        if (!supportedReadingTypes.Contains(Type))
        {
            Log.Error(@"ReadingModel: Unsupported reading type {Type}", Type);
            throw new NotSupportedException();
        }
        SerialNumber = jsonElement.GetProperty(@"serial_number").GetString() ?? string.Empty;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }
    public virtual ReadingModel AssignNewValues(ReadingModel reading)
    {
        JsonElementString = reading.JsonElementString;
        Type = reading.Type;
        SerialNumber = reading.SerialNumber;
        Timestamp = reading.Timestamp;
        return this;
    }
}
