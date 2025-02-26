namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Domain.Enumeration;

    using GamaEdtech.Common.Converter;

    using System.Text.Json.Serialization;

    public sealed class UserPermissionsResponseViewModel
    {
        [JsonConverter(typeof(FlagsEnumerationConverter<Role>))]
        public Role? Roles { get; set; }

        public IEnumerable<ClaimsResponseViewModel>? Claims { get; set; }
    }
}
