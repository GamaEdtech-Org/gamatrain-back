namespace GamaEdtech.Data.Dto.School
{
    public sealed class ManageSchoolCommentRequestDto
    {
        public long? Id { get; set; }
        public int SchoolId { get; set; }
        public int CreationUserId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string? Comment { get; set; }
        public double ClassesQualityRate { get; set; }
        public double EducationRate { get; set; }
        public double ITTrainingRate { get; set; }
        public double SafetyAndHappinessRate { get; set; }
        public double BehaviorRate { get; set; }
        public double TuitionRatioRate { get; set; }
        public double FacilitiesRate { get; set; }
        public double ArtisticActivitiesRate { get; set; }
    }
}
