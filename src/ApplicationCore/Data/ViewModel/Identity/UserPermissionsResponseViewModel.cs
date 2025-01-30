namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using GamaEdtech.Backend.Data.Enumeration;

    using Farsica.Framework.Converter;

    using System.Text.Json.Serialization;

    public sealed class UserPermissionsResponseViewModel
    {
        [JsonConverter(typeof(FlagsEnumerationConverter<Role>))]
        public Role? Roles { get; set; }

        public IEnumerable<ClaimsResponseViewModel>? Claims { get; set; }
    }
}
