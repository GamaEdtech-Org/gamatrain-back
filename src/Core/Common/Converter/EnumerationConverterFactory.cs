namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Data.Enumeration;

    public class EnumerationConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert([NotNull] Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Enumeration<,>);

        public override JsonConverter? CreateConverter([NotNull] Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = typeToConvert.GetGenericArguments()[0];
            if (elementType is null)
            {
                return null;
            }

            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(EnumerationConverter<,>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;

            return converter;
        }
    }
}
