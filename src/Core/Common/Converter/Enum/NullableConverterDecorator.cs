namespace GamaEdtech.Common.Converter.Enum
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public sealed class NullableConverterDecorator<T>(JsonConverter<T> innerConverter) : JsonConverter<T?>
        where T : struct
    {
        // Read() and Write() are never called with null unless HandleNull is overwridden -- which it is not.
        private readonly JsonConverter<T> innerConverter = innerConverter ?? throw new ArgumentNullException(nameof(innerConverter));

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => innerConverter.Read(ref reader, Nullable.GetUnderlyingType(typeToConvert)!, options);

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) => innerConverter.Write(writer, value!.Value, options);

        public override bool CanConvert([NotNull] Type typeToConvert) => base.CanConvert(typeToConvert) && innerConverter.CanConvert(Nullable.GetUnderlyingType(typeToConvert)!);
    }
}
