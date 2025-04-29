namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class RejectContributionRequestViewModel
    {
        [Display]
        [Required]
        public string? Comment { get; set; }
    }
}
