namespace GamaEdtech.Data.Dto.Provider.Core
{
    using System.Text.Json.Serialization;

    public sealed class CoreBoardResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("master_")]
        public string? Master { get; set; }

        [JsonPropertyName("list_order")]
        public string? Order { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("parent")]
        public string? Parent { get; set; }
    }
}
