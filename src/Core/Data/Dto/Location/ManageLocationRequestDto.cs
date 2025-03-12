namespace GamaEdtech.Data.Dto.Location
{
    using GamaEdtech.Domain.Enumeration;

    using NetTopologySuite.Geometries;

    public sealed class ManageLocationRequestDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? LocalTitle { get; set; }
        public string? Code { get; set; }
        public LocationType? LocationType { get; set; }
        public int? ParentId { get; set; }
        public Point? Coordinates { get; set; }
    }
}
