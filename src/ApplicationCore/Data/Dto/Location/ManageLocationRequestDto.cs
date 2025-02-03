namespace GamaEdtech.Backend.Data.Dto.Location
{
    using GamaEdtech.Backend.Data.Enumeration;

    public sealed class ManageLocationRequestDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? LocalTitle { get; set; }
        public string? Code { get; set; }
        public LocationType? LocationType { get; set; }
        public int? ParentId { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
    }
}
