namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Data.Enumeration;

    public class DictionaryEnumerationConverter<TKey> : JsonConverter<Dictionary<Enumeration<TKey>, object?>>
            where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        public override bool CanConvert([NotNull] Type typeToConvert) => typeof(Dictionary<Enumeration<TKey>, object?>) == typeToConvert;

        public override Dictionary<Enumeration<TKey>, object?> Read(ref Utf8JsonReader reader, Type typeToConvert, [NotNull] JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = new Dictionary<Enumeration<TKey>, object?>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();

                if (!propertyName.TryGetFromNameOrValue<Enumeration<TKey>, TKey>(out var enumeration) || enumeration is null)
                {
                    throw new JsonException($"Unable to convert \"{propertyName}\" to Enumeration \"{typeof(Enumeration<>)}\".");
                }

                // Get the value.
                _ = reader.Read();
                var value = (options.GetConverter(typeof(object)) as JsonConverter<object?>)?.Read(ref reader, typeof(object), options);

                // Add to dictionary.
                dictionary.Add(enumeration, value);
            }

            throw new JsonException();
        }

        public override void Write([NotNull] Utf8JsonWriter writer, [NotNull] Dictionary<Enumeration<TKey>, object?> dictionary, [NotNull] JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach ((var key, var value) in dictionary)
            {
                var propertyName = key.Name!;
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName);

                (options.GetConverter(typeof(object)) as JsonConverter<object?>)?.Write(writer, value, options);
            }

            writer.WriteEndObject();
        }
    }
}
