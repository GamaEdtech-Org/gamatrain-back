namespace GamaEdtech.Common.Converter.Enum
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Core.Extensions;

    public class JsonPropertyNameStringEnumConverter : GeneralJsonStringEnumConverter
    {
        public JsonPropertyNameStringEnumConverter()
            : base(default, true)
        {
        }

        public JsonPropertyNameStringEnumConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues)
            : base(namingPolicy, allowIntegerValues)
        {
        }

        protected override bool TryOverrideName(Type enumType, string? name, out ReadOnlyMemory<char> overrideName)
        {
            if (JsonEnumExtensions.TryGetEnumAttribute<JsonPropertyNameAttribute>(enumType, name, out var attr) && attr.Name is not null)
            {
                overrideName = attr.Name.AsMemory();
                return true;
            }

            return base.TryOverrideName(enumType, name, out overrideName);
        }
    }
}
