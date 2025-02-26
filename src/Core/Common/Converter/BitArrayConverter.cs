namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class BitArrayConverter : JsonConverter<BitArray>
    {
        public override BitArray? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<BitArrayDto>(ref reader, options)?.AsBitArray();

        public override void Write(Utf8JsonWriter writer, [NotNull] BitArray value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, new BitArrayDto(value), options);
    }
}
