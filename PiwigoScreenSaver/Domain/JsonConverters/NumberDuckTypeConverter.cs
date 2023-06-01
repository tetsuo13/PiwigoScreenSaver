using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PiwigoScreenSaver.Domain.JsonConverters;

/// <summary>
/// Allow both a string and a number on deserialize. Sometimes the "width"
/// and "height" properties values of an image will be a string.
/// </summary>
public class NumberDuckTypeConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (int.TryParse(reader.GetString(), out int value))
            {
                return value;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
