namespace GamaEdtech.Data.Dto.Provider.Core
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.HttpProvider;

    public sealed class CoreTestInformationResponse : IHttpResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("data")]
        public DataDto? Data { get; set; }

        public sealed class DataDto
        {
            [JsonPropertyName("true_answer")]
            public int AnswerId { get; set; }
        }
    }
}
