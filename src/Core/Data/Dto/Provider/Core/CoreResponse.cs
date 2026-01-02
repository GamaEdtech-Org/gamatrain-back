namespace GamaEdtech.Data.Dto.Provider.Core
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.HttpProvider;

    public sealed class CoreResponse<T> : IHttpResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
