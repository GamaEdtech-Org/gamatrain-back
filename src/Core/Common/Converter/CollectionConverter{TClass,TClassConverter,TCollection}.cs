namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public class CollectionConverter<TClass, TClassConverter, TCollection> : JsonConverter<TCollection>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
            where TCollection : IEnumerable<TClass>
            where TClassConverter : JsonConverter<TClass>, new()
    {
        private readonly JsonConverter<TClass> itemConverter = new TClassConverter();

        public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return (TCollection?)(object?)null;
            }

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                reader.Skip();
                return default;
            }

            var list = new List<TClass>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                var item = itemConverter.Read(ref reader, typeof(TClass), options);
                if (item is not null)
                {
                    list.Add(item);
                }
            }

            return typeof(TCollection) == typeof(TClass[]) ? (TCollection)(object)list.ToArray() : (TCollection)(object)list;
        }

        public override void Write([NotNull] Utf8JsonWriter writer, [NotNull] TCollection value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var item in value)
            {
                itemConverter.Write(writer, item, options);
            }

            writer.WriteEndArray();
        }
    }
}
