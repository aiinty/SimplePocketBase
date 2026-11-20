using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SimplePocketBase
{
    public class PocketBaseOptions
    {
        public JsonSerializerSettings JsonSettings { get; set; }

        internal static PocketBaseOptions CreateDefault()
        {
            return new PocketBaseOptions
            {
                JsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
        }
    }
}
