//using TempestMonitor.ViewModels.Observables;
using Microsoft.Maui.Controls;
using ArgumentException = System.ArgumentException;
using Exception = System.Exception; // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
using JsonTokenType = System.Text.Json.JsonTokenType;
using Type = System.Type; // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
namespace TempestMonitor.Models;

public abstract class DatabaseBaseModel
{
    [Column("Id"), PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }
    [Column("json_document")]
    public string json_document { get; set; } = string.Empty;

    public abstract void DeserializeAndSend();
}
public abstract class DatabaseBaseModel_VW
{
    [Column("Id"), PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }
    [Column("json_document")]
    public string json_document { get; set; } = string.Empty;
}

public class BasePersistenceObject
{
    public required string TableName { get; set; }
    public string JsonDocument { get; set; } = string.Empty;
}

public class TableAndReadingTypeToDataAssociation
{
    //ToDo: Make as much as possible private
    public string TableName { get; set; } = string.Empty;
    public string ReadingType { get; set; } = string.Empty;
    public Type DataType { get; set; } = typeof(DatabaseBaseModel);
    public Type? ViewType { get; set; } = typeof(DatabaseBaseModel);
    public Type? ObservableViewType { get; set; } = typeof(ObservableObject);

    public DatabaseBaseModel? DataTypeInstance { get; private set; } = null;

    private static TableAndReadingTypeToDataAssociation? GetAssociationsByReadingType(string readingType)
    {
        return TableToClassMappings.Where(x => x.ReadingType is not null).FirstOrDefault(x => x.ReadingType == readingType);
    }
    public static TableAndReadingTypeToDataAssociation? CreateDataTypeInstance(string readingType, string jsonDocument)
    {
        var tableToClass = GetAssociationsByReadingType(readingType);
        if (tableToClass is null)
        {
            var message = $"No mapping found for reading type: {readingType}";
            Log.Error(message);
            throw new ArgumentException(message);
        }

        var dataTypeInstance = (DatabaseBaseModel?)Activator.CreateInstance(tableToClass.DataType);
        if (dataTypeInstance is null)
        {
            var message = $"Unable to create instance of type: {tableToClass.DataType}";
            Log.Error(message);
            throw new ArgumentException(message);
        }

        dataTypeInstance.json_document = jsonDocument;
        tableToClass.DataTypeInstance = dataTypeInstance;

        return tableToClass;
    }
    public static TableAndReadingTypeToDataAssociation? CreateDataTypeInstanceFromTableName(string tableName, string jsonDocument)
    {
        var tableToClass = TableToClassMappings.FirstOrDefault(x => x.TableName == tableName);
        if (tableToClass is null)
        {
            var message = $"No mapping found for table: {tableName}";
            Log.Error(message);
            throw new ArgumentException(message);

        }

        var dataTypeInstance = (DatabaseBaseModel?)Activator.CreateInstance(tableToClass.DataType);
        if (dataTypeInstance is null)
        {
            var message = $"Unable to create instance of type: {tableToClass.DataType}";
            Log.Error(message);
            throw new ArgumentException(message);
        }

        dataTypeInstance.json_document = jsonDocument;
        tableToClass.DataTypeInstance = dataTypeInstance;

        return tableToClass;
    }

    public static TableAndReadingTypeToDataAssociation[] TableToClassMappings { get; set; } =
    [
        new TableAndReadingTypeToDataAssociation
        {
            TableName = "DeviceStatus",
            ReadingType = "device_status",
            DataType = typeof(DeviceStatusModel),
            ViewType = null,
            ObservableViewType = typeof(ObservableVW_WindModel)
        },

        new TableAndReadingTypeToDataAssociation
        {
            TableName = "HubStatus",
            ReadingType = "hub_status",
            DataType = typeof(HubStatusModel),
            ViewType = null,
            ObservableViewType = null
        },

        new TableAndReadingTypeToDataAssociation
        {
            TableName = "LightningStrike",
            ReadingType = "evt_strike",
            DataType = typeof(LightningStrikeModel),
            ViewType = null,
            ObservableViewType = null
        },

        new TableAndReadingTypeToDataAssociation
        {
            TableName = "Observation",
            ReadingType = "obs_st",
            DataType = typeof(ObservationModel),
            ViewType = typeof(VW_ObservationModel),
            ObservableViewType = typeof(ObservableVW_ObservationModel)
        },

        new TableAndReadingTypeToDataAssociation
        {
            TableName = "RainStart",
            ReadingType = "evt_precip",
            DataType = typeof(RainStartModel),
            ViewType = null,
            ObservableViewType = null
        },

        new TableAndReadingTypeToDataAssociation
        {
            TableName = "WeatherForecast",
            ReadingType = "weather_forecast",       // Not really needed, never used, not a reading type
            DataType = typeof(WeatherForecastModel),
            ViewType = typeof(WeatherForecastGraph),
            ObservableViewType = typeof(ObservableWeatherForecastGraph)
        },

        new TableAndReadingTypeToDataAssociation
        {
            TableName = "Wind",
            ReadingType = "rapid_wind",
            DataType = typeof(WindModel),
            ViewType = typeof(VW_WindModel),
            ObservableViewType = typeof(ObservableVW_WindModel)
        },
    ];
}

[Table("DeviceStatus")]
public class DeviceStatusModel : DatabaseBaseModel
{
    public const string type = "device_status";
    public override void DeserializeAndSend()
    {
        return;
    }
}

[Table("HubStatus")]
public class HubStatusModel : DatabaseBaseModel
{
    public const string type = "hub_status";
    public override void DeserializeAndSend()
    {
        return;
    }
}
[Table("LightningStrike")]
public class LightningStrikeModel : DatabaseBaseModel
{
    public const string type = "evt_strike";
    public override void DeserializeAndSend()
    {
        return;
    }
}
[Table("Observation")]
public class ObservationModel : DatabaseBaseModel
{
    public const string type = "obs_st";
    public override void DeserializeAndSend()
    {
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions { Converters = { new VW_ObservationModelDeserializer() } };
            var vw_ObservationModel = JsonSerializer.Deserialize<VW_ObservationModel>(json_document, options);
            if (vw_ObservationModel is not null)
                WeakReferenceMessenger.Default.Send(new VW_Message<VW_ObservationModel>(vw_ObservationModel));
            else
            {
                var message = $"vw_ObservationModel is null after Deserialize call";
                Log.Error(message);
                throw new ArgumentException(message);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Exception deserializing ObservationModel {json_document}");
        }
    }

   public class VW_ObservationModelDeserializer : System.Text.Json.Serialization.JsonConverter<VW_ObservationModel>
    {
        private double GetNextDouble(ref System.Text.Json.Utf8JsonReader reader)
        {
            reader.Read();
            return reader.GetDouble();
        }
        private long GetNextLong(ref System.Text.Json.Utf8JsonReader reader)
        {
            reader.Read();           
            return reader.GetInt64();
        }
        public override VW_ObservationModel Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var vw_ObservationModel = new VW_ObservationModel();
            reader.Read();  // serial_number property name
            reader.Read();  // serial_number property value
            reader.Read();  // type property name
            reader.Read();  // type property value
            reader.Read();  // hub_sn property name
            reader.Read();  // hub_sn property value
            vw_ObservationModel.hub_sn = reader.GetString()!;
            reader.Read();  // obs array name
            reader.Read();  // start array?
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new System.Text.Json.JsonException("Expected StartArray token.");
            }

            reader.Read();  // StartObject?
            // DO NOT CHANGE THE ORDER OF THESE It's the order they appear in the array where each element is not named!
            vw_ObservationModel.timestamp_local = GetNextLong(ref reader);
            vw_ObservationModel.wind_lull = GetNextDouble(ref reader);
            vw_ObservationModel.wind_average = GetNextDouble(ref reader);
            vw_ObservationModel.wind_gust = GetNextDouble(ref reader);
            vw_ObservationModel.wind_direction = GetNextLong(ref reader);
            vw_ObservationModel.wind_sample_interval = GetNextLong(ref reader);
            vw_ObservationModel.station_pressure = GetNextDouble(ref reader);
            vw_ObservationModel.air_temperature = GetNextDouble(ref reader);
            vw_ObservationModel.relative_humidity = GetNextDouble(ref reader);
            vw_ObservationModel.illuminance = GetNextDouble(ref reader);
            vw_ObservationModel.uv = GetNextDouble(ref reader);
            vw_ObservationModel.solar_radiation = GetNextDouble(ref reader);
            vw_ObservationModel.rain_accumulation_over_the_previous_minute = GetNextDouble(ref reader);
            vw_ObservationModel.precipitation_type = GetNextLong(ref reader);
            vw_ObservationModel.lightning_strike_average_distance = GetNextDouble(ref reader);
            vw_ObservationModel.lightning_strike_count = GetNextLong(ref reader);
            vw_ObservationModel.battery = GetNextDouble(ref reader);
            vw_ObservationModel.reporting_interval = GetNextLong(ref reader);

            reader.Read();      // EndArray of array of numbers/Observation
            reader.Read();      // EndArray of array of array of numbers/Observations
            reader.Read();      // firmware_revision property name
            vw_ObservationModel.firmware_revision = GetNextLong(ref reader);
            reader.Read();      // EndObject

            return vw_ObservationModel;
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, VW_ObservationModel value, System.Text.Json.JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
[Table("RainStart")]
public class RainStartModel : DatabaseBaseModel
{
    public const string type = "evt_precip";
    public override void DeserializeAndSend()
    {
        return;
    }
}
[Table("WeatherForecast")]
public class WeatherForecastModel : DatabaseBaseModel
{
    public override void DeserializeAndSend()
    {
        try
        {
            var viewTypeInstance = JsonSerializer.Deserialize<WeatherForecastGraph>(json_document);
            if (viewTypeInstance is not null)
                WeakReferenceMessenger.Default.Send(new VW_Message<WeatherForecastGraph>(viewTypeInstance));
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Exception deserializing WeatherForecastModel {json_document}");
        }
    }
}
[Table("Wind")]
public class WindModel : DatabaseBaseModel
{
    public const string type = "rapid_wind";
    public const string table = "Wind";

    public override void DeserializeAndSend()
    {
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions { Converters = { new VW_WindModelDeserializer() } };
            var viewTypeInstance = JsonSerializer.Deserialize<VW_WindModel>(json_document, options);
            if (viewTypeInstance is not null)
                WeakReferenceMessenger.Default.Send(new VW_Message<VW_WindModel>(viewTypeInstance));
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Exception deserializing WindModel {json_document}");
        }
    }
    public class VW_WindModelDeserializer : System.Text.Json.Serialization.JsonConverter<VW_WindModel>
    {
        private double GetNextDouble(ref System.Text.Json.Utf8JsonReader reader)
        {
            reader.Read();
            return reader.GetDouble();
        }

        private long GetNextLong(ref System.Text.Json.Utf8JsonReader reader)
        {
            reader.Read();
            return reader.GetInt64();
        }
        public override VW_WindModel Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var vw_WindModel = new VW_WindModel();
            reader.Read();  // StartObject
            reader.Read();  // serial_number property name
            reader.Read();  // serial_number property value
            reader.Read();  // type property name
            reader.Read();  // type property value
            reader.Read();  // hub_sn property name
            reader.Read();  // hub_sn property value
            reader.Read();  // ob array name
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new System.Text.Json.JsonException("Expected StartArray token.");
            }

            // DO NOT CHANGE THE ORDER OF THESE It's the order they appear in the array where each element is not named!
            vw_WindModel.timestamp_local = GetNextLong(ref reader);
            vw_WindModel.wind_speed = GetNextDouble(ref reader);
            vw_WindModel.wind_direction = GetNextLong(ref reader);

            reader.Read();      // EndArray of array
            reader.Read();      // EndObject

            return vw_WindModel;
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, VW_WindModel value, System.Text.Json.JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

