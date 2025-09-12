namespace GamaEdtech.Data.Dto.Provider.CryptoCurrency
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.HttpProvider;

    public sealed class GamaTrainConvertResponse : IHttpResponse
    {
        [JsonPropertyName("outAmount")]
        public decimal Amount { get; set; }
    }
}
