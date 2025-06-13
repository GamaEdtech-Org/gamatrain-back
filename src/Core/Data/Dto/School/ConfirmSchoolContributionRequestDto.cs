namespace GamaEdtech.Data.Dto.School
{
    public sealed class ConfirmSchoolContributionRequestDto
    {
        public required long ContributionId { get; set; }
        public long? SchoolId { get; set; }
        public required SchoolContributionDto Data { get; set; }
    }
}
