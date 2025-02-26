namespace GamaEdtech.Common.Swagger
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.OpenApi.Models;

    using NUlid;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class UlidToStringSchemaFilter : ISchemaFilter
    {
        public void Apply([NotNull] OpenApiSchema schema, [NotNull] SchemaFilterContext context)
        {
            var isUlidType = context.Type == typeof(Ulid);
            if (!isUlidType)
            {
                return;
            }

            schema.Type = "string";
            schema.Properties = null;
            schema.AllOf = null;
        }
    }
}
