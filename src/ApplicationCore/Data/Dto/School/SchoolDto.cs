namespace GamaEdtech.Backend.Data.Dto.School
{
    using GamaEdtech.Backend.Data.Enumeration;

    public sealed class SchoolDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LocalName { get; set; }
        public required SchoolType SchoolType { get; set; }
        public int StateId { get; set; }
        public string? StateTitle { get; set; }
        public string? ZipCode { get; set; }
        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
