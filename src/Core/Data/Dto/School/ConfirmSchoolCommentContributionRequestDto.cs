namespace GamaEdtech.Data.Dto.School
{
    public sealed class ConfirmSchoolCommentContributionRequestDto
    {
        public required long ContributionId { get; set; }
        public required bool NotifyUser { get; set; }
    }
}
