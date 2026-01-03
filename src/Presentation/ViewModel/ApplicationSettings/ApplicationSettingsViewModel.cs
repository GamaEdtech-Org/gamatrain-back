namespace GamaEdtech.Presentation.ViewModel.ApplicationSettings
{
    public sealed class ApplicationSettingsViewModel
    {
        public int GridPageSize { get; set; }
        public string? DefaultTimeZoneId { get; set; }
        public long SchoolContributionPoints { get; set; }
        public long SchoolImageContributionPoints { get; set; }
        public long SchoolCommentContributionPoints { get; set; }
        public long PostContributionPoints { get; set; }
        public long SchoolIssuesContributionPoints { get; set; }
        public long RemoveSchoolImageContributionPoints { get; set; }
        public long EasterEggBronzePoints { get; set; }
        public long EasterEggSilverPoints { get; set; }
        public long EasterEggGoldPoints { get; set; }
        public long TestTimeCorrectSubmissionPoints { get; set; }
        public long TestTimeIncorrectSubmissionPoints { get; set; }
        public long ExamCorrectTestSubmissionPoints { get; set; }
        public long ExamIncorrectTestSubmissionPoints { get; set; }
    }
}
