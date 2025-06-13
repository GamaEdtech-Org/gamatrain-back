namespace GamaEdtech.Presentation.ViewModel.School
{
    public sealed class SchoolsResponseViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? LocalName { get; set; }
        public Uri? DefaultImageUri { get; set; }
    }
}
