namespace GamaEdtech.Data.Dto.Provider.Core
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.HttpProvider;

    public sealed class CoreUserInformationResponse : IHttpResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("data")]
        public DataDto? Data { get; set; }

        public sealed class DataDto
        {
            [JsonPropertyName("first_name")]
            public string? FirstName { get; set; }

            [JsonPropertyName("last_name")]
            public string? LastName { get; set; }

            [JsonPropertyName("avatar")]
            public string? Avatar { get; set; }

            [JsonPropertyName("phone")]
            public string? Phone { get; set; }

            [JsonPropertyName("sex")]
            public string? Sex { get; set; }

            [JsonPropertyName("base")]
            public string? Grade { get; set; }

            [JsonPropertyName("group_id")]
            public int? Group { get; set; }

            [JsonPropertyName("id")]
            public string? CoreId { get; set; }
        }
    }
}
