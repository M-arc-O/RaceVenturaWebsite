using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RaceVenturaAPI.Helpers
{
    public class TrimmingJsonConverter : JsonConverter<string>
    {
        public override bool CanConvert(Type objType)
        {
            return objType == typeof(string);
        }

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(string))
            {
                if (!string.IsNullOrEmpty(reader.GetString()))
                {
                    return reader.GetString().Trim();
                }
            }
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            if (string.IsNullOrEmpty(value))
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Trim());
            }
        }
    }
}
