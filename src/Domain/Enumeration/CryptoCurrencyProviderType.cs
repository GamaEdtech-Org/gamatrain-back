namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class CryptoCurrencyProviderType : Enumeration<CryptoCurrencyProviderType, byte>
    {
        [Display]
        public static readonly CryptoCurrencyProviderType GamaTrain = new(nameof(GamaTrain), 0);

        public CryptoCurrencyProviderType()
        {
        }

        public CryptoCurrencyProviderType(string name, byte value) : base(name, value)
        {
        }
    }
}
