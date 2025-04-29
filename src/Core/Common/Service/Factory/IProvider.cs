namespace GamaEdtech.Common.Service.Factory
{
    using GamaEdtech.Common.Data.Enumeration;

    public interface IProvider<T>
        where T : Enumeration<T, byte>
    {
        T ProviderType { get; }
    }
}
