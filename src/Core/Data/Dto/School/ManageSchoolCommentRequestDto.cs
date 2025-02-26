namespace GamaEdtech.Data.Dto.School
{
    public sealed class ManageSchoolCommentRequestDto
    {
        public long? Id { get; set; }
        public int SchoolId { get; set; }
        public int CreationUserId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string? Comment { get; set; }
        public byte ClassesQualityRate { get; set; }
        public byte EducationRate { get; set; }
        public byte ITTrainingRate { get; set; }
        public byte SafetyAndHappinessRate { get; set; }
        public byte BehaviorRate { get; set; }
        public byte TuitionRatioRate { get; set; }
        public byte FacilitiesRate { get; set; }
        public byte ArtisticActivitiesRate { get; set; }
    }
}
