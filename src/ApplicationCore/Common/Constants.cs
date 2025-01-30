
namespace GamaEdtech.Backend.Common
{
    using System.Text.Json.Serialization;

    using Farsica.Framework.Converter;

    public static class Constants
    {
        [JsonConverter(typeof(EnumStringConverter<EntityType>))]
        public enum EntityType
        {
            ApplicationSetting = 0,
            ApplicationUser = 1,
            ApplicationUserClaim = 2,
        }
    }
}
