using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Models
{
    public class DbModelRole  : BaseModel
    {
       

        public List<BaseModel> Permissions { get; set; }

        public  DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int Version { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}