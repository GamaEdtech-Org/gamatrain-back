namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data.Enumeration;

    public class FlagsEnumerationConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : FlagsEnumeration<TEnum>, new()
    {
        public override bool CanConvert([NotNull] Type typeToConvert) => typeToConvert.IsSubclassOf(typeof(FlagsEnumeration<TEnum>)) || Globals.IsSubclassOf(typeToConvert, typeof(FlagsEnumeration<TEnum>));

        public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return (TEnum?)(object?)null;
            }

            if (reader.TokenType is not JsonTokenType.StartArray)
            {
                reader.Skip();
                return reader.GetString()?.FromName<TEnum>() ?? reader.GetString()?.FromUniqueId<TEnum>();
            }

            var list = new List<string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                var val = reader.GetString();
                if (string.IsNullOrEmpty(val))
                {
                    continue;
                }

                list.Add(val);
            }

            return list.ListToFlagsEnum<TEnum>();
        }

        public override void Write([NotNull] Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options) => writer.WriteRawValue(JsonSerializer.Serialize(value.GetNames()));
    }
}
