namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ConfirmSchoolContributionRequestViewModel
    {
        [Display]
        [Required]
        public int? SchoolId { get; set; }
    }
}
