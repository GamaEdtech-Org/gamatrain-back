namespace GamaEdtech.Data.Dto.Identity
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class ProfileSettingsDto
    {
        public string? TimeZoneId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public long? SchoolId { get; set; }
        public string? ReferralId { get; set; }
        public GenderType? Gender { get; set; }
        public int? Board { get; set; }
        public int? Grade { get; set; }
        public int? Group { get; set; }
        public int? CoreId { get; set; }
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
        public bool ProfileUpdated { get; set; }
    }
}
