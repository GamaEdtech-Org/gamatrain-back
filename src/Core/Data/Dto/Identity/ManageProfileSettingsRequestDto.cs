namespace GamaEdtech.Data.Dto.Identity
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class ManageProfileSettingsRequestDto
    {
        public required int UserId { get; set; }
        public int? CityId { get; set; }
        public long? SchoolId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public GenderType? Gender { get; set; }
        public int? Board { get; set; }
        public int? Grade { get; set; }
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
    }
}
