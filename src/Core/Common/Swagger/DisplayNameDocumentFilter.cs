namespace GamaEdtech.Common.Swagger
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class DisplayNameDocumentFilter : IDocumentFilter
    {
        public void Apply([NotNull] OpenApiDocument swaggerDoc, [NotNull] DocumentFilterContext context)
        {
            swaggerDoc.Tags ??= [];

            foreach (var item in context.ApiDescriptions)
            {
                if (item.ActionDescriptor is not ControllerActionDescriptor descriptor)
                {
                    continue;
                }

                var displayAttribute = descriptor.ControllerTypeInfo.GetCustomAttribute<DisplayAttribute>();
                var description = displayAttribute is null ? string.Empty : displayAttribute.Name;

                var area = descriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.AreaName;
                if (string.IsNullOrEmpty(area))
                {
                    area = descriptor.ControllerTypeInfo.GetCustomAttribute<Microsoft.AspNetCore.Mvc.AreaAttribute>()?.RouteValue;
                }

                var name = string.IsNullOrEmpty(area) ? descriptor.ControllerName : $"{area} - {descriptor.ControllerName}";

                swaggerDoc.Tags.Add(new OpenApiTag
                {
                    Name = name,
                    Description = description,
                });
            }

            swaggerDoc.Tags = [.. swaggerDoc.Tags.OrderBy(t => t.Name)];
        }
    }
}
