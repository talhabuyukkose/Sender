using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sender.Core.Extensions.JsonProcess
{
    public static class JsonProcess
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            IgnoreNullValues = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public static string JsonSerialize(this object _object)
        {
            return JsonSerializer.Serialize(_object, _options);
        }

        public static T JsonDeserialize<T>(this string _string)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(_string, _options);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to deserialize string to type " + typeof(T).Name, ex);
            }
        }
        public static T JsonDeserialize<T>(this object _object)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(_object.JsonSerialize(), _options);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to deserialize string to type " + typeof(T).Name, ex);
            }
        }

        public class UpperCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) => name.ToUpper();
        }
    }
}
