using Newtonsoft.Json;

namespace TravelCompanion.Domain.Extensions
{
    public static class JsonExtensions
    {
        public static string ConvertToJson(this object value, Formatting formatting = Formatting.Indented, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(value, formatting, settings ?? new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
