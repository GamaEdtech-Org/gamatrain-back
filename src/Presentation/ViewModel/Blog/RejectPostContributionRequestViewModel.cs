namespace GamaEdtech.Presentation.ViewModel.Blog
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class RejectPostContributionRequestViewModel
    {
        [Display]
        [Required]
        public string? Comment { get; set; }
    }
}
