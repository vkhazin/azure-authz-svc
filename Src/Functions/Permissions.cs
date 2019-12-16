using System;
using System.Linq;
using System.Threading.Tasks;
using AzureAuthorizationFunctionApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureAuthorizationFunctionApp.Functions
{
    public static class Permissions
    {
        [FunctionName("Permissions")]
        public static async Task<ObjectResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Permissions/{parameter}")] HttpRequest req,
            string parameter,ILogger log)
        {
            var correlationId = req.Headers.Any(r => r.Key == "x-correlation-id")
                ? req.Headers["x-correlation-id"].ToString()
                : string.Empty;

            var accessToken = req.Headers.Any(r => r.Key == "x-access-token")
                ? req.Headers["x-access-token"].ToString()
                : string.Empty;

            if (string.IsNullOrEmpty(parameter)
                || string.IsNullOrEmpty(accessToken))
            {
                return new BadRequestObjectResult($"Missing parameters. Correlation ID: {correlationId}");
            }

            try
            {
                var userDetails = await AzTokenController.Instance.GetTokenDetails(accessToken);
                var isValidAccessToken = userDetails != null && userDetails.userPrincipalName != "";

                if (!isValidAccessToken)
                {
                    return new BadRequestObjectResult($"Invalid access token provided. Correlation ID: {correlationId}");
                }

                var roles = parameter.Split(',');

                return new OkObjectResult(CosmosDbController.Instance.GetUserPermissions(accessToken));
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                return new BadRequestObjectResult($"Error occured. Please contact the system administrator. Correlation ID: {correlationId}");
            }

        }
    }
}
