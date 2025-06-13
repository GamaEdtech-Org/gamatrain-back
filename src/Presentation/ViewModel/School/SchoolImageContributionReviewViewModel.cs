namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class SchoolImageContributionReviewViewModel
    {
        public long Id { get; set; }
        public string? SchoolName { get; set; }
        public long SchoolId { get; set; }
        public string? FileId { get; set; }
        public FileType? FileType { get; set; }
        public bool IsDefault { get; set; }
        public long? TagId { get; set; }
        public string? TagName { get; set; }
    }
}
