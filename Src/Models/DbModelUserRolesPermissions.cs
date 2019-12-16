using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Models
{
    public class DbModelUserRolesPermissions : BaseModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public List<BaseModel> Roles { get; set; }

        public List<BaseModel> Permissions { get; set; }

        public DateTime Created { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}