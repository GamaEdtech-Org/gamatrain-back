namespace GamaEdtech.Data.Dto.Provider.CryptoCurrency
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.HttpProvider;
    using GamaEdtech.Domain.Enumeration;

    public sealed class GamaTrainTransactionDetailsResponse : IHttpResponse
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("message")]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("memo")]
        public string? Memo { get; set; }

        [JsonPropertyName("transfer")]
        public TransferDto? Transfer { get; set; }
    }

    public sealed class TransferDto
    {
        [JsonPropertyName("sourceWallet")]
        public string? SourceWallet { get; set; }

        [JsonPropertyName("destinationWallet")]
        public string? DestinationWallet { get; set; }

        [JsonPropertyName("amount")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("mint")]
        public string? Mint { get; set; }

        [JsonPropertyName("symbol")]
        [JsonConverter(typeof(EnumerationConverter<Currency, byte>))]
        public Currency? Currency { get; set; }
    }
}
