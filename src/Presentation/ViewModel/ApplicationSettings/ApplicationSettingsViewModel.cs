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
    }
}
