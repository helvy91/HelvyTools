using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using NewtonsoftJson = Newtonsoft.Json;

namespace HelvyTools.Utils.Json
{
    public static class JsonUtils
    {
        public static T Deserialize<T>(string json, bool caseInsensitive = true)
        {
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = caseInsensitive;

            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static string Serialize<T>(T value, bool caseInsensitive = false)
        {
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = caseInsensitive;
            options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            return JsonSerializer.Serialize(value, options);
        }

        public static string SerializeRecursive<T>(T value)
        {
            return NewtonsoftJson.JsonConvert.SerializeObject(value);
        }

        public static T DeserializeRecursive<T>(string json)
        {
            return NewtonsoftJson.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
