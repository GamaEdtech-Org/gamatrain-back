namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ManageSchoolCommentRequestViewModel
    {
        [Display]
        [Required]
        public string? Comment { get; set; }

        [Display]
        public byte ClassesQualityRate { get; set; }

        [Display]
        public byte EducationRate { get; set; }

        [Display]
        public byte ITTrainingRate { get; set; }

        [Display]
        public byte SafetyAndHappinessRate { get; set; }

        [Display]
        public byte BehaviorRate { get; set; }

        [Display]
        public byte TuitionRatioRate { get; set; }

        [Display]
        public byte FacilitiesRate { get; set; }

        [Display]
        public byte ArtisticActivitiesRate { get; set; }
    }
}
