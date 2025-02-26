namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Data.Enumeration;

    public class EnumerationConverter<TEnum, TKey> : JsonConverter<TEnum>
        where TEnum : Enumeration<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        private const string NameProperty = "Name";

        public override bool CanConvert([NotNull] Type typeToConvert) => typeToConvert.IsSubclassOf(typeof(Enumeration<TKey>));

        public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return (TEnum?)(object?)null;
            }

            string? value = null;
            if (typeToConvert == typeof(byte))
            {
                value = reader.GetByte().ToString();
            }
            else if (typeToConvert == typeof(short))
            {
                value = reader.GetInt16().ToString();
            }
            else if (typeToConvert == typeof(int))
            {
                value = reader.GetInt32().ToString();
            }
            else if (typeToConvert == typeof(long))
            {
                value = reader.GetInt64().ToString();
            }
            else if (typeToConvert == typeof(double))
            {
                value = reader.GetDouble().ToString();
            }

            return reader.TokenType switch
            {
                JsonTokenType.Number => GetEnumerationFromJson(value),
                JsonTokenType.String => GetEnumerationFromJson(reader.GetString()),
                JsonTokenType.Null => null,
                _ => throw new JsonException($"Unexpected token {reader.TokenType} when parsing the enumeration."),
            };
        }

        public override void Write([NotNull] Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNull(NameProperty);
                return;
            }

            var name = value.GetType().GetProperty(NameProperty, BindingFlags.Public | BindingFlags.Instance) ?? throw new JsonException($"Error while writing JSON for {value}");
            writer.WriteStringValue(name.GetValue(value)?.ToString());
        }

        private static TEnum? GetEnumerationFromJson(string? nameOrValue)
        {
            _ = nameOrValue.TryGetFromNameOrValue<TEnum, TKey>(out var result);

            return result;
        }
    }
}
