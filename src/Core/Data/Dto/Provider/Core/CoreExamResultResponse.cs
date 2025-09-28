namespace GamaEdtech.Data.Dto.Provider.Core
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.HttpProvider;

    public sealed class CoreExamResultResponse : IHttpResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public DataDto? Data { get; set; }

        public sealed class DataDto
        {
            [JsonPropertyName("answerStats")]
            public AnswerStatsDto? AnswerStats { get; set; }

            public sealed class AnswerStatsDto
            {
                [JsonPropertyName("total")]
                public TotalDto? Total { get; set; }

                public sealed class TotalDto
                {
                    [JsonPropertyName("num")]
                    public int Num { get; set; }

                    [JsonPropertyName("true")]
                    public int True { get; set; }

                    [JsonPropertyName("false")]
                    public int False { get; set; }

                    [JsonPropertyName("noAnswer")]
                    public int NoAnswer { get; set; }

                    [JsonPropertyName("percent")]
                    public double Percent { get; set; }
                }
            }
        }
    }
}
