namespace GamaEdtech.Data.Dto.School
{
    public sealed class RemoveSchoolImageContributionDto
    {
        public long SchoolId { get; set; }
        public string? FileId { get; set; }
        public string? Description { get; set; }
    }
}
