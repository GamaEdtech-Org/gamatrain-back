namespace GamaEdtech.Presentation.ViewModel.Game
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Domain.Enumeration;

    public sealed class CoinViewModel
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(EnumerationConverter<CoinType, byte>))]
        public CoinType CoinType { get; set; }

        public DateTimeOffset ExpirationTime { get; set; }
    }
}
