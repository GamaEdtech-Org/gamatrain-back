namespace GamaEdtech.Data.Dto.Provider.Core
{
    using System.Text.Json.Serialization;

    public sealed class CoreTestInformationResponse
    {
        [JsonPropertyName("true_answer")]
        public int AnswerId { get; set; }
    }
}
