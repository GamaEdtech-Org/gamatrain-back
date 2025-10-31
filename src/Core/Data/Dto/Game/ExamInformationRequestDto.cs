namespace GamaEdtech.Data.Dto.Game
{
    public sealed class ExamInformationRequestDto
    {
        public required long ExamId { get; set; }
        public required string? SecretKey { get; set; }
    }
}
