namespace AzureAuthorizationFunctionApp.Models
{
    public abstract class PermissionModel
    {
        public string Key { get; set; }

        public bool IsAuthorized { get; set; }
    }
}