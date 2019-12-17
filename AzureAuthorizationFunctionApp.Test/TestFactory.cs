using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

namespace AzureAuthorizationFunctionApp.Test
{
    /// <summary>Unit Tests Class</summary>
    public class TestFactory
    {

        /// <summary>Creates the HTTP request.</summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <returns></returns>
        public static DefaultHttpRequest CreateHttpRequest(string accessToken, string correlationId)

        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Headers.Add("x-correlation-id", correlationId);
            request.Headers.Add("x-access-token", accessToken);

            return request;
        }

        /// <summary>Creates the logger.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            var logger = type == LoggerTypes.List ? new ListLogger() : NullLoggerFactory.Instance.CreateLogger("Null Logger");

            return logger;
        }
    }
}
