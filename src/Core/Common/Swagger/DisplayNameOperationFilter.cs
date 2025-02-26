namespace GamaEdtech.Common.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class DisplayNameOperationFilter : IOperationFilter
    {
        public void Apply([NotNull] OpenApiOperation operation, [NotNull] OperationFilterContext context)
        {
            IEnumerable<object> actionAttributes = context.MethodInfo is null ? [] : context.MethodInfo.GetCustomAttributes(true);
            IEnumerable<object> metadataAttributes = context.ApiDescription?.ActionDescriptor?.EndpointMetadata is null ? Array.Empty<object>() : context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var actionAndEndpointAttribtues = actionAttributes.Union(metadataAttributes).Distinct();
            ApplySwaggerOperationAttribute(operation, actionAndEndpointAttribtues);

            if (context.ApiDescription?.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.AreaName;
                if (string.IsNullOrEmpty(area))
                {
                    area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<Microsoft.AspNetCore.Mvc.AreaAttribute>()?.RouteValue;
                }

                operation.Tags = string.IsNullOrEmpty(area)
                    ? [new OpenApiTag { Name = controllerActionDescriptor.ControllerName }]
                    : [new OpenApiTag { Name = $"{area} - {controllerActionDescriptor.ControllerName}" }];
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
