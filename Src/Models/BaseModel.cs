using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Models
{
    public class BaseModel
    {
        [JsonProperty(PropertyName = "key")]
        public string key { get; set; }
    }
}