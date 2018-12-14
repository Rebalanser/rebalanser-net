using System;

namespace Rebalanser.Core
{
    public class Providers
    {
        private static Func<IRebalanserProvider> GetRebalanserProvider;

        public static void Register(Func<IRebalanserProvider> getRebalanserProvider)
        {
            GetRebalanserProvider = getRebalanserProvider;
        }

        public static IRebalanserProvider GetProvider()
        {
            if (GetRebalanserProvider == null)
                throw new ProviderException("No provider registered!");

            return GetRebalanserProvider();
        }
    }
}
