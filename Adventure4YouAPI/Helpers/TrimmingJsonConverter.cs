using Newtonsoft.Json;
using System;

namespace Adventure4YouAPI.Helpers
{
    public class TrimmingJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objType)
        {
            return objType == typeof(string);
        }

        public override object ReadJson(JsonReader objJsonReader, Type objType, object obj, JsonSerializer objJsonSerializer)
        {
            if (objJsonReader.TokenType == JsonToken.String)
            {
                if (objJsonReader.Value != null)
                {
                    return (objJsonReader.Value as string).Trim();
                }
            }
            return objJsonReader.Value;
        }

        public override void WriteJson(JsonWriter objJsonWriter, object obj, JsonSerializer objJsonSerializer)
        {
            string str = (string)obj;
            if (str == null)
            {
                objJsonWriter.WriteNull();
            }
            else
            {
                objJsonWriter.WriteValue(str.Trim());
            }
        }
    }
}
