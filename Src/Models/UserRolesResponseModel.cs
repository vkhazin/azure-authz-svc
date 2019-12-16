using System.Collections.Generic;

namespace AzureAuthorizationFunctionApp.Models
{
    /// <summary>A class to model the response of Roles Azure function</summary>
    public class UserRolesResponseModel
    {
        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        /// <summary>Gets or sets the correlation identifier.</summary>
        /// <value>The correlation identifier.</value>
        public string CorrelationId { get; set; }

        /// <summary>Gets or sets the timestamp.</summary>
        /// <value>The timestamp.</value>
        public string Timestamp { get; set; }

        /// <summary>Gets or sets the roles.</summary>
        /// <value>The roles.</value>
        public IList<RoleResponseModel> Roles { get; set; }
    }
}