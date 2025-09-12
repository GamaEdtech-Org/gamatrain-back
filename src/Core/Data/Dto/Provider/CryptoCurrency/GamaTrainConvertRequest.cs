namespace GamaEdtech.Data.Dto.Provider.CryptoCurrency
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.HttpProvider;

    public sealed class GamaTrainConvertRequest : IHttpRequest
    {
        [JsonPropertyName("inputMint")]
        public string? SourceMint { get; set; }

        [JsonPropertyName("outputMint")]
        public string? DestinationMint { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("slippageBps")]
        public decimal SlippageBps { get; set; } = 50;

        [JsonPropertyName("onlyDirectRoutes")]
        public bool OnlyDirectRoutes { get; set; }

        [JsonPropertyName("asLegacyTransaction")]
        public bool AsLegacyTransaction { get; set; }
    }
}
