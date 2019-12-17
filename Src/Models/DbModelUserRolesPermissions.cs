using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Models
{
    /// <summary>A model class for CosmosDb UserRolesPermissions item</summary>
    /// <seealso cref="AzureAuthorizationFunctionApp.Models.DbModelUserRolesPermissions" />
    public class DbModelUserRolesPermissions : BaseModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        /// <summary>Gets or sets the list of roles.</summary>
        /// <value>The roles.</value>
        public List<BaseModel> Roles { get; set; }

        /// <summary>Gets or sets the list of permissions.</summary>
        /// <value>The permissions.</value>
        public List<BaseModel> Permissions { get; set; }

        /// <summary>Gets or sets the created date.</summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}