using System;
using System.Collections.Generic;
using AzureAuthorizationFunctionApp.Functions;
using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Models
{
    public class DbModelUser : BaseModel
    {
        public int Version { get; set; }

        public List<BaseModel> Roles { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}