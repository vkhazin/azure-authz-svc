namespace AzureAuthorizationFunctionApp.Models
{
    /// <summary>A ´model calss used to model the permission object part of the Azure function response</summary>
    /// <seealso cref="AzureAuthorizationFunctionApp.Models.BaseModel" />
    public class RoleResponseModel : BaseModel
    {
        /// <summary>Gets or sets a value indicating whether this instance is authorized or not.</summary>
        /// <value>
        ///   <c>true</c> if this instance is authorized; otherwise, <c>false</c>.</value>
        public bool IsAuthorized { get; set; }
    }
}