namespace GamaEdtech.Presentation.ViewModel.Location
{
    public sealed class LocationResponseViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? LocalTitle { get; set; }
        public string? Code { get; set; }
        public int? ParentId { get; set; }
        public string? ParentTitle { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
    }
}
