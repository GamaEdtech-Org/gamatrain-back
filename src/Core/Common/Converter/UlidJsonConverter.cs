namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Resources;

    using NUlid;

    public class UlidJsonConverter : JsonConverter<Ulid>
    {
        public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.String)
            {
                throw new ArgumentException("TokenType");
            }

            var val = reader.GetString() ?? string.Empty;
            var txt = GlobalResource.Validation_ValueIsInvalidAccessor;
            return Ulid.TryParse(val, out var ulid)
                ? ulid
                : throw new ArgumentException(string.Format(txt, val));
        }

        public override void Write([NotNull] Utf8JsonWriter writer, Ulid value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());

        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(Ulid);
    }
}
