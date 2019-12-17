using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Models
{
    /// <summary>A model class for Cosmosdb User item</summary>
    /// <seealso cref="AzureAuthorizationFunctionApp.Models.DbModelUser" />
    public class DbModelUser : BaseModel
    {
        /// <summary>Gets or sets the version.</summary>
        /// <value>The version.</value>
        public int Version { get; set; }

        /// <summary>Gets or sets the list of roles.</summary>
        /// <value>The roles.</value>
        public List<BaseModel> Roles { get; set; }

        /// <summary>Gets or sets the created date.</summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified.</value>
        public DateTime Modified { get; set; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}