namespace GamaEdtech.Data.Dto.School
{
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;

    public sealed class CreateSchoolImageRequestDto
    {
        public required IFormFile File { get; set; }
        public required FileType FileType { get; set; }
        public int SchoolId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int CreationUserId { get; set; }
    }
}
