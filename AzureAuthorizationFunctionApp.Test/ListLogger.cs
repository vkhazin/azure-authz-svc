using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AzureAuthorizationFunctionApp.Test
{

    public class ListLogger : ILogger
    {
        private readonly IList<string> _logs;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => false;

        /// <summary>Initializes a new instance of the <see cref="ListLogger"/> class.</summary>
        public ListLogger()
        {
            this._logs = new List<string>();
        }

        public void Log<TState>(LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);
            this._logs.Add(message);
        }
    }
}
