namespace GamaEdtech.Data.Dto.School
{
    public sealed class ConfirmSchoolImageContributionRequestDto
    {
        public required long ContributionId { get; set; }
        public required bool NotifyUser { get; set; }
    }
}
