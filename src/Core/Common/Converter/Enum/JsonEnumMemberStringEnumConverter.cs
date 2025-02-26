namespace GamaEdtech.Common.Converter.Enum
{
    using System;
    using System.Text.Json;

    using GamaEdtech.Common.Core.Extensions;

    public class JsonEnumMemberStringEnumConverter : GeneralJsonStringEnumConverter
    {
        public JsonEnumMemberStringEnumConverter()
            : base(default, true)
        {
        }

        public JsonEnumMemberStringEnumConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues)
            : base(namingPolicy, allowIntegerValues)
        {
        }

        protected override bool TryOverrideName(Type enumType, string? name, out ReadOnlyMemory<char> overrideName)
        {
            if (JsonEnumExtensions.TryGetEnumAttribute<System.Runtime.Serialization.EnumMemberAttribute>(enumType, name, out var attr) && attr.Value is not null)
            {
                overrideName = attr.Value.AsMemory();
                return true;
            }

            return base.TryOverrideName(enumType, name, out overrideName);
        }
    }
}
