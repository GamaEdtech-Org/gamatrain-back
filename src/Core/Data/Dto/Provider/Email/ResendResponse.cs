namespace GamaEdtech.Data.Dto.Provider.Email
{
    using System.Text.Json.Serialization;

    public class ResendResponse<T>
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreationDate { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
