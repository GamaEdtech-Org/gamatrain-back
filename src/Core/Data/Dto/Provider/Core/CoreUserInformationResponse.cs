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
            public Sex? Sex { get; set; }

            [JsonPropertyName("base")]
            public int? Grade { get; set; }
        }

#pragma warning disable CA1008 // Enums should have zero value
        public enum Sex
#pragma warning restore CA1008 // Enums should have zero value
        {
            Male = 1,
            Female = 2,
        }
    }
}
