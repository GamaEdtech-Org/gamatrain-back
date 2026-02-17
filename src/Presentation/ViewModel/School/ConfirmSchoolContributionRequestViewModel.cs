namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ConfirmSchoolContributionRequestViewModel
    {
        [Display]
        public long? SchoolId { get; set; }
    }
}
