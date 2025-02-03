namespace GamaEdtech.Backend.Data.Dto.School
{
    using GamaEdtech.Backend.Data.Enumeration;

    public sealed class ManageSchoolRequestDto
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string LocalName { get; set; }
        public required SchoolType SchoolType { get; set; }
        public required int StateId { get; set; }
        public required string ZipCode { get; set; }
        public required string Address { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
}
