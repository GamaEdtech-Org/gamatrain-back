namespace GamaEdtech.Data.Dto.School
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class SchoolImageContributionDto
    {
        public long SchoolId { get; set; }
        public string? FileId { get; set; }
        public FileType? FileType { get; set; }
        public long? TagId { get; set; }
        public bool IsDefault { get; set; }
    }
}
