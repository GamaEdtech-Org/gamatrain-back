namespace GamaEdtech.Data.Dto.Blog
{
    public sealed class ConfirmPostContributionRequestDto
    {
        public required long ContributionId { get; set; }
        public required bool NotifyUser { get; set; }
    }
}
