namespace GamaEdtech.Data.Dto.School
{
    public sealed class SchoolsDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? LocalName { get; set; }
        public Uri? DefaultImageUri { get; set; }
    }
}
