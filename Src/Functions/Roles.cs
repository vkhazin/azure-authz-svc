using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AzureAuthorizationFunctionApp.Controllers;
using AzureAuthorizationFunctionApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureAuthorizationFunctionApp.Functions
{
    public static class Roles
    {
        [FunctionName("Roles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Roles/{parameter}")] HttpRequest req, string parameter, ILogger log)
        {
            var correlationId = req.Headers.Any(r => r.Key == "x-correlation-id") ? req.Headers["x-correlation-id"].ToString() : string.Empty;

            var accessToken = req.Headers.Any(r => r.Key == "x-access-token") ? req.Headers["x-access-token"].ToString() : string.Empty;

            if (string.IsNullOrEmpty(parameter)
                || string.IsNullOrEmpty(accessToken))
            {
                return new BadRequestObjectResult($"Missing parameters. Correlation ID: {correlationId}");
            }


            try
            {
                var userDetails = await AzTokenController.Instance.GetTokenDetails(accessToken);
                var isValidAccessToken = userDetails != null && !string.IsNullOrEmpty(userDetails.userPrincipalName);

                if (!isValidAccessToken)
                {
                    // Should return 401 
                    return new BadRequestObjectResult($"Invalid access token provided. Correlation ID: {correlationId}");
                }


                var userRolesPermissions = await CosmosDbController.Instance.GetUserRolesPermissions(userDetails);

                if (userRolesPermissions == null)
                    return new OkObjectResult("No permissions defined for the specified token"); // Should return 401 

                var inputRoles = parameter.Split(',');

                var response = new UserRolesResponseModel()
                {
                    UserId = userRolesPermissions.key,
                    CorrelationId = correlationId,
                    Timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    Roles = new List<RoleModel>()
                };

                foreach (var role in inputRoles)
                {
                    response.Roles.Add(userRolesPermissions.Roles.Any(g => g.key == role)
                        ? new RoleModel { Key = role, IsAuthorized = true }
                        : new RoleModel { Key = role, IsAuthorized = false });
                }


                return new JsonResult(response);
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                return new BadRequestObjectResult($"Error occured. Please contact the system administrator. Correlation ID: {correlationId}");
            }
        }
    }
}
