namespace GamaEdtech.Presentation.ViewModel.School
{
    public sealed class RemoveSchoolImageContributionReviewViewModel
    {
        public long Id { get; set; }

        public string? SchoolName { get; set; }

        public long SchoolId { get; set; }

        public Uri? FileUri { get; set; }
    }
}
