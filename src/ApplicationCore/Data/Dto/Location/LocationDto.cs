namespace GamaEdtech.Backend.Data.Dto.Location
{
    public sealed class LocationDto
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
