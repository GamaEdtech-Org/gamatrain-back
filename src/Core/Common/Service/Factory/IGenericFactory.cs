namespace GamaEdtech.Common.Service.Factory
{
    using GamaEdtech.Common.Data.Enumeration;

    [DataAnnotation.Injectable]
    public interface IGenericFactory<TProvider, TProviderType>
        where TProvider : IProvider<TProviderType>
        where TProviderType : Enumeration<byte>
    {
        TProvider? GetProvider(TProviderType providerType, bool returnFirstItemIfNotMatch = false);
    }
}
