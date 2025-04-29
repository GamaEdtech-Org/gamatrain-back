namespace GamaEdtech.Presentation.ViewModel.School
{
    public sealed class SchoolCommentContributionReviewViewModel
    {
        public long Id { get; set; }
        public string? SchoolName { get; set; }
        public long SchoolId { get; set; }
        public string? Comment { get; set; }
        public double ArtisticActivitiesRate { get; set; }
        public double BehaviorRate { get; set; }
        public double ClassesQualityRate { get; set; }
        public double EducationRate { get; set; }
        public double FacilitiesRate { get; set; }
        public double ITTrainingRate { get; set; }
        public double SafetyAndHappinessRate { get; set; }
        public double TuitionRatioRate { get; set; }
        public double AverageRate { get; set; }
    }
}
