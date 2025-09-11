namespace GamaEdtech.Presentation.ViewModel.Identity
{
    public sealed class ProfileSettingsResponseViewModel
    {
        public string? UserName { get; set; }
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
        public Uri? Avatar { get; set; }
    }
}
