namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ManageSchoolCommentRequestViewModel
    {
        [Display]
        [Required]
        public string? Comment { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double ClassesQualityRate { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double EducationRate { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double ITTrainingRate { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double SafetyAndHappinessRate { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double BehaviorRate { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double TuitionRatioRate { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double FacilitiesRate { get; set; }

        [Display]
        [Range(0, 5.0)]
        public double ArtisticActivitiesRate { get; set; }
    }
}
