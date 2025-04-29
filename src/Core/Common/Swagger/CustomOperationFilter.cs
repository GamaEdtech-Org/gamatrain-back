namespace GamaEdtech.Common.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class CustomOperationFilter : IOperationFilter
    {
        public void Apply([NotNull] OpenApiOperation operation, [NotNull] OperationFilterContext context)
        {
            IEnumerable<object> actionAttributes = context.MethodInfo is null ? [] : context.MethodInfo.GetCustomAttributes(true);
            IEnumerable<object> metadataAttributes = context.ApiDescription?.ActionDescriptor?.EndpointMetadata is null ? Array.Empty<object>() : context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var actionAndEndpointAttribtues = actionAttributes.Union(metadataAttributes).Distinct();
            ApplySwaggerOperationAttribute(operation, actionAndEndpointAttribtues);

            if (context.ApiDescription?.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
            {
                return;
            }

            var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.AreaName;
            if (string.IsNullOrEmpty(area))
            {
                area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<Microsoft.AspNetCore.Mvc.AreaAttribute>()?.RouteValue;
            }
            operation.Tags = string.IsNullOrEmpty(area)
                ? [new OpenApiTag { Name = controllerActionDescriptor.ControllerName }]
                : [new OpenApiTag { Name = $"{area} - {controllerActionDescriptor.ControllerName}" }];


            PrepaireEnumerations();

            void PrepaireEnumerations()
            {
                var queryParams = operation.Parameters.Where(t => t.In == ParameterLocation.Query);
                if (!queryParams.Any())
                {
                    return;
                }

                for (var i = 0; i < controllerActionDescriptor.Parameters.Count; i++)
                {
                    var property = controllerActionDescriptor.Parameters[i];
                    if (property.ParameterType.IsPrimitive)
                    {
                        continue;
                    }

                    var properties = property.ParameterType.GetProperties();
                    for (var j = 0; j < properties.Length; j++)
                    {
                        var item = properties[j];

                        var isEnumeration = Globals.IsSubclassOf(item.PropertyType, typeof(Enumeration<,>));
                        if (!isEnumeration && !Globals.IsSubclassOf(item.PropertyType, typeof(FlagsEnumeration<>)))
                        {
                            continue;
                        }

                        var serializedList = JsonSerializer.Serialize(operation.Parameters.Where(t => !t.Name.StartsWith(item.Name + ".", StringComparison.OrdinalIgnoreCase)));
                        var list = JsonSerializer.Deserialize<IEnumerable<OpenApiParameter>>(serializedList)!;
                        operation.Parameters.Clear();
                        foreach (var param in list)
                        {
                            operation.Parameters.Add(param);
                        }

                        var names = EnumerationExtensions.GetNames(item.PropertyType)!
                            .Select(t => new OpenApiString(t)).ToList<IOpenApiAny>();
                        var apiParameter = new OpenApiParameter
                        {
                            Name = item.Name,
                            In = ParameterLocation.Query,
                            Schema = new OpenApiSchema
                            {
                                Properties = null,
                                AllOf = null,
                            },
                        };

                        if (isEnumeration)
                        {
                            apiParameter.Schema.Enum = names;
                            apiParameter.Schema.Type = "string";
                        }
                        else
                        {
                            apiParameter.Schema.Properties = null;
                            apiParameter.Schema.AllOf = null;
                            apiParameter.Schema.Type = "array";
                            apiParameter.Schema.Items = new OpenApiSchema
                            {
                                Enum = names,
                                Type = "string",
                                Properties = null,
                                AllOf = null,
                            };
                        }

                        operation.Parameters.Add(apiParameter);
                    }
                }
            }
        }

        private static void ApplySwaggerOperationAttribute(OpenApiOperation operation, IEnumerable<object> actionAttributes)
        {
            var displayAttribute = actionAttributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (displayAttribute is null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(displayAttribute.Name))
            {
                operation.Summary = displayAttribute.Name;
            }
        }
    }
}
