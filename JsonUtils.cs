using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace roslyingscriptingdemo
{
    internal static class JsonUtils
    {
        private static JsonSerializerSettings JsonSettings => new()
        {
            ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() },
        };

        internal static string Stringify(object data)
        {
            return JsonConvert.SerializeObject(data, JsonSettings);
        }

    }
}
