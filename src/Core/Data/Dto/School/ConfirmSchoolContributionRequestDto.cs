namespace GamaEdtech.Data.Dto.School
{
    public sealed class ConfirmSchoolContributionRequestDto
    {
        public required long ContributionId { get; set; }
        public required bool NotifyUser { get; set; }
        public long? SchoolId { get; set; }
    }
}
