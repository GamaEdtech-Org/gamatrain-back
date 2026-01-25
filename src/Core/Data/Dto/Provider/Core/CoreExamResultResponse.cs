namespace GamaEdtech.Data.Dto.Provider.Core
{
    using System.Text.Json.Serialization;

    public sealed class CoreExamResultResponse
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
