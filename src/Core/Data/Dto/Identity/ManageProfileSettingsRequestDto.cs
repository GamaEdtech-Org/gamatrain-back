namespace GamaEdtech.Data.Dto.Identity
{
    using Microsoft.AspNetCore.Http;

    public sealed class ManageProfileSettingsRequestDto
    {
        public required int UserId { get; set; }
        public int? CityId { get; set; }
        public long? SchoolId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public int? Section { get; set; }
        public int? Grade { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
    }
}
