namespace GamaEdtech.Data.Dto.Provider.CryptoCurrency
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.HttpProvider;

    public sealed class GamaTrainTransactionDetailsRequest : IHttpRequest
    {
        [JsonPropertyName("txid")]
        public string? TransactionId { get; set; }

        [JsonPropertyName("apiKey")]
        public string? ApiKey { get; set; }
    }
}
