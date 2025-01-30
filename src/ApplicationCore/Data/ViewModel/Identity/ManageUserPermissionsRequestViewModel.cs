namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using GamaEdtech.Backend.Data.Enumeration;

    using Farsica.Framework.Converter;
    using Farsica.Framework.DataAnnotation;

    using System.Text.Json.Serialization;

    public sealed class ManageUserPermissionsRequestViewModel
    {
        [Display]
        public IEnumerable<string>? Claims { get; set; }

        [Display]
        [JsonConverter(typeof(FlagsEnumerationConverter<Role>))]
        public Role? Roles { get; set; }
    }
}
