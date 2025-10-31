namespace GamaEdtech.Data.Dto.Game
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class ExportExamRequestDto
    {
        public required int UserId { get; set; }
        public required string? Url { get; set; }
        public required string? SecretKey { get; set; }
        public required long ExamId { get; set; }
        public required ExportFileType FileType { get; set; }
        public string? Watermark { get; set; }
        public int? Duration { get; set; }
    }
}
