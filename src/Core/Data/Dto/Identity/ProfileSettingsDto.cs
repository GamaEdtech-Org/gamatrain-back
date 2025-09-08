namespace GamaEdtech.Data.Dto.Identity
{
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
        public string? Gender { get; set; }
        public int? Section { get; set; }
        public int? Grade { get; set; }
    }
}
