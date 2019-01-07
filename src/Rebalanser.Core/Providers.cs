using System;

namespace Rebalanser.Core
{
    /// <summary>
    /// Registers functions that create Rebalanser providers
    /// </summary>
    public class Providers
    {
        private static Func<IRebalanserProvider> GetRebalanserProvider;

        public static void Register(Func<IRebalanserProvider> getRebalanserProvider)
        {
            GetRebalanserProvider = getRebalanserProvider;
        }

        /// <summary>
        /// Called from inside the RebalanserClient
        /// </summary>
        /// <returns>An IRebalanserProvider implementation</returns>
        /// <exception cref="ProviderException"></exception>
        public static IRebalanserProvider GetProvider()
        {
            if (GetRebalanserProvider == null)
                throw new ProviderException("No provider registered!");

            return GetRebalanserProvider();
        }
    }
}
