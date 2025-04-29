namespace GamaEdtech.Common.Service.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GamaEdtech.Common.Data.Enumeration;

    public class GenericFactory<TProvider, TProviderType>(IEnumerable<TProvider> providers) : IGenericFactory<TProvider, TProviderType>
        where TProvider : IProvider<TProviderType>
        where TProviderType : Enumeration<TProviderType, byte>
    {
        public TProvider? GetProvider(TProviderType providerType, bool returnFirstItemIfNotMatch = false)
        {
            ArgumentNullException.ThrowIfNull(providers);

            var provider = providers.FirstOrDefault(t => t.ProviderType.Equals(providerType));
            if (provider is null && returnFirstItemIfNotMatch)
            {
                provider = providers.First();
            }

            return provider;
        }
    }
}
