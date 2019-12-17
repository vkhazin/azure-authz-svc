using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Models
{
    /// <summary>A base model class</summary>
    public class BaseModel
    {
        /// <summary>Gets or sets the key.</summary>
        /// <value>The key.</value>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
}