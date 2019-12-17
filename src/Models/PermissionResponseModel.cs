namespace AzureAuthorizationFunctionApp.Models
{
    public class PermissionResponseModel
    {
        public string Key { get; set; }

        public bool IsAuthorized { get; set; }
    }
}