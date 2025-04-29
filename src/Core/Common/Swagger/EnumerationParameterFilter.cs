namespace GamaEdtech.Common.Swagger
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class EnumerationParameterFilter(Type type) : IParameterFilter
    {
        public void Apply([NotNull] OpenApiParameter parameter, [NotNull] ParameterFilterContext context)
        {
            var routeInfo = context.ApiParameterDescription.RouteInfo;
            if (routeInfo?.Constraints != null && routeInfo.Constraints.Any(t => t.GetType().Name == type.Name))
            {
                var lst = EnumerationExtensions.GetNames(type);
                if (lst is not null)
                {
                    parameter.Schema.Enum = [.. lst.Select(t => new OpenApiString(t))];
                    parameter.Schema.Type = "string";
                    parameter.Schema.Properties = null;
                    parameter.Schema.AllOf = null;
                }
            }
        }
    }
}
