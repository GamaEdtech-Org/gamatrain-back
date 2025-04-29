namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Domain.Enumeration;

    using GamaEdtech.Common.Converter;

    using System.Text.Json.Serialization;

    public sealed class UserPermissionsResponseViewModel
    {
        [JsonConverter(typeof(FlagsEnumerationConverter<Role>))]
        public Role? Roles { get; set; }

        [JsonConverter(typeof(FlagsEnumerationConverter<SystemClaim>))]
        public SystemClaim? SystemClaims { get; set; }

        public IEnumerable<PermissionsResponseViewModel>? Permissions { get; set; }
    }
}
