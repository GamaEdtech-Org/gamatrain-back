namespace GamaEdtech.Data.Dto.School
{
    using System;

    public sealed class SchoolCommentContributionDto
    {
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
        public DateTimeOffset CreationDate { get; set; }
        public int CreationUserId { get; set; }
    }
}
