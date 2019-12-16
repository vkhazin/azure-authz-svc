using System.Collections.Generic;

namespace AzureAuthorizationFunctionApp.Models
{
    public class UserRolesResponseModel
    {
        public string UserId { get; set; }

        public string CorrelationId { get; set; }

        public string Timestamp { get; set; }

        public IList<RoleModel> Roles { get; set; }
    }
}