namespace GamaEdtech.Presentation.ViewModel.ApplicationSettings
{
    public sealed class ApplicationSettingsViewModel
    {
        public int GridPageSize { get; set; }
        public string? DefaultTimeZoneId { get; set; }
        public int SchoolContributionPoints { get; set; }
        public int SchoolImageContributionPoints { get; set; }
        public int SchoolCommentContributionPoints { get; set; }
        public int PostContributionPoints { get; set; }
    }
}
