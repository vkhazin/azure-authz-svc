namespace AzureAuthorizationFunctionApp.Controllers
{
    public class UserDetails
    {
        /// <summary>Gets or sets the name of the user principal.</summary>
        /// <value>The name of the user principal.</value>
        public string UserPrincipalName { get; set; }
        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public string Id { get; set; }

        /// <summary>Gets or sets the user email address.</summary>
        /// <value>The user email.</value>
        public string Mail { get; set; }
    }
}