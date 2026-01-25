namespace GamaEdtech.Data.Dto.ApplicationSettings
{
    public sealed class ApplicationSettingsDto
    {
        public int GridPageSize { get; set; } = 10;
        public string? DefaultTimeZoneId { get; set; }
        public long SchoolContributionPoints { get; set; }
        public long SchoolImageContributionPoints { get; set; }
        public long SchoolCommentContributionPoints { get; set; }
        public long PostContributionPoints { get; set; }
        public long SchoolIssuesContributionPoints { get; set; }
        public long RemoveSchoolImageContributionPoints { get; set; }
        public long EasterEggBronzePoints { get; set; } = 1000000;
        public long EasterEggSilverPoints { get; set; } = 3000000;
        public long EasterEggGoldPoints { get; set; } = 6000000;
        public long TestTimeCorrectSubmissionPoints { get; set; } = 10;
        public long TestTimeIncorrectSubmissionPoints { get; set; } = 10;
        public long ExamCorrectTestSubmissionPoints { get; set; } = 1000;
        public long ExamIncorrectTestSubmissionPoints { get; set; } = 1000;
    }
}
