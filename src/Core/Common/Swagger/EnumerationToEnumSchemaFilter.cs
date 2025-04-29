namespace GamaEdtech.Common.Swagger
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class EnumerationToEnumSchemaFilter : ISchemaFilter
    {
        public void Apply([NotNull] OpenApiSchema schema, [NotNull] SchemaFilterContext context)
        {
            var isEnumeration = Globals.IsSubclassOf(context.Type, typeof(Enumeration<,>));
            if (!isEnumeration && !Globals.IsSubclassOf(context.Type, typeof(FlagsEnumeration<>)))
            {
                return;
            }

            if (isEnumeration)
            {
                schema.Enum = [.. EnumerationExtensions.GetNames(context.Type)!.Select(t => new OpenApiString(t))];
                schema.Type = "string";
                schema.Properties = null;
                schema.AllOf = null;
            }
            else
            {
                schema.Properties = null;
                schema.AllOf = null;
                schema.Type = "array";
                schema.Items = new OpenApiSchema
                {
                    Enum = [.. FlagsEnumerationExtensions.GetNames(context.Type)!.Select(t => new OpenApiString(t))],
                    Type = "string",
                    Properties = null,
                    AllOf = null,
                };
            }
        }
    }
}
