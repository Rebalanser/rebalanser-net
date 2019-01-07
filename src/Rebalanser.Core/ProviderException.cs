using System;

namespace Rebalanser.Core
{
    /// <summary>
    /// An exception that indicates a problem instantiating a provider
    /// </summary>
    public class ProviderException : Exception
    {
        public ProviderException(string message)
            : base(message)
        {

        }
    }
}
