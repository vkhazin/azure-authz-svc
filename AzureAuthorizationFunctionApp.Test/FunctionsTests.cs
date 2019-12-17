using System;
using AzureAuthorizationFunctionApp.Functions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AzureAuthorizationFunctionApp.Test
{
    /// <summary>Unit Tests Class</summary>
    public class FunctionsTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();

        /// <summary>  Verify if Roles Function the should return missing access token.</summary>
        [Fact]
        public async void Roles_should_return_missing_access_token()
        {
            var correlationId = Guid.NewGuid().ToString();
            var request = TestFactory.CreateHttpRequest(string.Empty,correlationId);
            var response = (ObjectResult)await Roles.Run(request,"", _logger);
            Assert.Equal($"Missing Access Token. Correlation ID: {correlationId}", response.Value);
        }

        /// <summary>  Verify if Roles Function should missing parameters validation.</summary>
        [Fact]
        public async void Roles_should_return_missing_parameters()
        {
            var correlationId = Guid.NewGuid().ToString();
            var accessToken = Guid.NewGuid().ToString();
            var request = TestFactory.CreateHttpRequest(accessToken, correlationId);
            var response = (ObjectResult)await Roles.Run(request, "", _logger);
            Assert.Equal($"Missing parameters. Correlation ID: {correlationId}", response.Value);

        }


        /// <summary>Verify if Permissions Function should return missing access token validation.</summary>
        [Fact]
        public async void Permissions_should_return_missing_access_token()
        {
            var correlationId = Guid.NewGuid().ToString();
            var request = TestFactory.CreateHttpRequest(string.Empty, correlationId);
            var response = (ObjectResult)await Permissions.Run(request, "", _logger);
            Assert.Equal($"Missing Access Token. Correlation ID: {correlationId}", response.Value);
        }

        /// <summary>Verify if Permissions Function should return missing parameters validation.</summary>
        [Fact]
        public async void Permissions_should_return_missing_parameters()
        {
            var correlationId = Guid.NewGuid().ToString();
            var accessToken = Guid.NewGuid().ToString();
            var request = TestFactory.CreateHttpRequest(accessToken, correlationId);
            var response = (ObjectResult)await Permissions.Run(request, "", _logger);
            Assert.Equal($"Missing parameters. Correlation ID: {correlationId}", response.Value);

        }
    }
}
