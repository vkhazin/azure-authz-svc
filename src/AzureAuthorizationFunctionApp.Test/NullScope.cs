using System;

namespace AzureAuthorizationFunctionApp.Test
{
    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        /// <summary>Prevents a default instance of the <see cref="NullScope"/> class from being created.</summary>
        private NullScope() { }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() { }
    }
}
