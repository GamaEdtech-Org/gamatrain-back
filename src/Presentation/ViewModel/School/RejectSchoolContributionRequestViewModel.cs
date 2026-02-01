namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class RejectSchoolContributionRequestViewModel
    {
        [Display]
        [Required]
        public string? Comment { get; set; }
    }
}
