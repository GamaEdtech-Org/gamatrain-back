namespace GamaEdtech.Data.Dto.Game
{
    public sealed class ExamPointsRequestDto
    {
        public required int UserId { get; set; }
        public required long ExamId { get; set; }
        public required string? SecretKey { get; set; }
    }
}
