namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    public sealed class ManageUserPermissionsRequestViewModel
    {
        [Display]
        public IEnumerable<string>? Permissions { get; set; }

        [Display]
        [JsonConverter(typeof(FlagsEnumerationConverter<SystemClaim>))]
        public SystemClaim? SystemClaims { get; set; }

        [Display]
        [JsonConverter(typeof(FlagsEnumerationConverter<Role>))]
        public Role? Roles { get; set; }
    }
}
