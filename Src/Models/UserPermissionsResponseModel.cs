using System.Collections.Generic;


namespace AzureAuthorizationFunctionApp.Models
{
    public class UserPermissionsResponseModel
    {
        public string UserId { get; }
        public string CorrelationId { get; }

        public string Timestamp { get; }

        public IList<PermissionModel> Permissions { get; }
    }
}