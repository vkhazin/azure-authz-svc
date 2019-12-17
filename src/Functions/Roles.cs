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
    /// <summary>Base class for Roles Azure Function</summary>
    public static class Roles
    {
        [FunctionName("Roles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Roles/{parameter}")]
            HttpRequest req, string parameter, ILogger log)
        {
            var correlationId = req.Headers.Any(r => r.Key == "x-correlation-id")
                ? req.Headers["x-correlation-id"].ToString()
                : string.Empty;

            var accessToken = req.Headers.Any(r => r.Key == "x-access-token")
                ? req.Headers["x-access-token"].ToString()
                : string.Empty;

            if (string.IsNullOrEmpty(accessToken))
            {
                return new ObjectResult($"Missing Access Token. Correlation ID: {correlationId}")
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
            }

            if (string.IsNullOrEmpty(parameter))
            {
                return new BadRequestObjectResult($"Missing parameters. Correlation ID: {correlationId}");
            }

            try
            {
                var userDetails = await AzTokenController.Instance.GetTokenDetails(accessToken);
                var isValidAccessToken = userDetails != null && !string.IsNullOrEmpty(userDetails.UserPrincipalName);

                if (!isValidAccessToken)
                {
                    return new ObjectResult($"Invalid access token provided. Correlation ID: {correlationId}")
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                    };
                }

                var cosmosDbController = new CosmosDbController(log);

                var userRolesPermissions = await cosmosDbController.GetUserRolesPermissions(userDetails);

                if (userRolesPermissions == null)
                    return new ObjectResult(
                        $"No permissions defined for the specified token. Correlation ID: {correlationId}")
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                    };

                var inputRoles = parameter.Split(',');

                var response = new UserRolesResponseModel()
                {
                    UserId = userRolesPermissions.Key,
                    CorrelationId = correlationId,
                    Timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    Roles = new List<RoleResponseModel>()
                };

                foreach (var role in inputRoles)
                {
                    response.Roles.Add(userRolesPermissions.Roles.Any(g => g.Key == role)
                        ? new RoleResponseModel {Key = role, IsAuthorized = true}
                        : new RoleResponseModel {Key = role, IsAuthorized = false});
                }

                return new JsonResult(response);
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                return new ObjectResult(
                    $"Error occured. Please contact the system administrator. Correlation ID: {correlationId}")
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
