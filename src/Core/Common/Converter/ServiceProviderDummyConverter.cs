namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using Microsoft.AspNetCore.Http;

    public class ServiceProviderDummyConverter(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider) : JsonConverter<object>
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public IHttpContextAccessor HttpContextAccessor { get; } = httpContextAccessor;

        public IServiceProvider ServiceProvider => HttpContextAccessor.HttpContext?.RequestServices ?? serviceProvider;

        public override bool CanConvert(Type typeToConvert) => false;

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotSupportedException();

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) => throw new NotSupportedException();
    }
}
