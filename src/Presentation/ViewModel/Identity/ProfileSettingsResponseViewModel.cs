namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Domain.Enumeration;

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
        public int? Board { get; set; }
        public int? Grade { get; set; }
        public int? Group { get; set; }
        public int? CoreId { get; set; }
        public string? Avatar { get; set; }
        public string? WalletId { get; set; }
        public bool ProfileUpdated { get; set; }

        [JsonConverter(typeof(FlagsEnumerationConverter<Role>))]
        public Role? Roles { get; set; }
    }
}
