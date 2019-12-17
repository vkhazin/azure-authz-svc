using System.Collections.Generic;


namespace AzureAuthorizationFunctionApp.Models
{
    /// <summary>A class to model the response of Azure Function Permissions</summary>
    public class UserPermissionsResponseModel
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

        /// <summary>Gets or sets the list of permissions.</summary>
        /// <value>The permissions.</value>
        public IList<PermissionResponseModel> Permissions { get; set; }
    }
}