namespace GamaEdtech.Data.Dto.School
{
    using NetTopologySuite.Geometries;

    public sealed class SchoolInfoDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public Point? Coordinates { get; set; }
        public string? CityTitle { get; set; }
        public string? CountryTitle { get; set; }
        public string? StateTitle { get; set; }
        public bool HasWebSite { get; set; }
        public bool HasPhoneNumber { get; set; }
        public bool HasEmail { get; set; }
        public DateTimeOffset LastModifyDate { get; set; }
        public double? Score { get; set; }
        public double? Distance { get; set; }
        public long? DefaultImageId { get; set; }
        public Uri? DefaultImageUri { get; set; }
    }
}
