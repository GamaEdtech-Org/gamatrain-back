namespace GamaEdtech.Common.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class EnumerationToEnumSchemaFilter : ISchemaFilter
    {
        public void Apply([NotNull] OpenApiSchema schema, [NotNull] SchemaFilterContext context)
        {
            var isEnumeration = IsSubclassOf(typeof(Enumeration<>), context.Type);
            if (!isEnumeration && !IsSubclassOf(typeof(FlagsEnumeration<>), context.Type))
            {
                return;
            }

            var fields = context.Type.GetFields(BindingFlags.Static | BindingFlags.Public);

            if (isEnumeration)
            {
                schema.Enum = [.. fields.Select(t => new OpenApiString(t.Name)).Cast<IOpenApiAny>()];
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
                    Enum = [.. fields.Select(t => new OpenApiString(t.Name)).Cast<IOpenApiAny>()],
                    Type = "string",
                    Properties = null,
                    AllOf = null,
                };
            }
        }

        private static bool IsSubclassOf(Type? generic, Type? toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}
