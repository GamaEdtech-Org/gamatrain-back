namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class EnumStringConverter<T> : JsonConverter<T>
    {
        private readonly JsonConverter<T>? converter;
        private readonly Type underlyingType;

        public EnumStringConverter()
            : this(null)
        {
        }

        public EnumStringConverter(JsonSerializerOptions? options)
        {
            if (options is not null)
            {
                converter = options.GetConverter(typeof(T)) as JsonConverter<T>;
            }

            var type = Nullable.GetUnderlyingType(typeof(T));
            underlyingType = type is null ? typeof(T) : type;
        }

        public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return (T?)(object?)null;
            }

            if (converter is not null)
            {
                return converter.Read(ref reader, underlyingType, options);
            }

            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            // for performance, parse with ignoreCase:false first.
            return System.Enum.TryParse(underlyingType, value, ignoreCase: false, out var result)
                || System.Enum.TryParse(underlyingType, value, ignoreCase: true, out result)
                ? (T?)result
                : throw new JsonException($"Unable to convert \"{value}\" to Enum \"{underlyingType}\".");
        }

        public override void Write([NotNull] Utf8JsonWriter writer, T? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString());
    }
}
