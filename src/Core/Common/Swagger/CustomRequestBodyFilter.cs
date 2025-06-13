namespace GamaEdtech.Common.Swagger
{
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class CustomRequestBodyFilter : IRequestBodyFilter
    {
        public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
        {
            if (context is null || requestBody is null)
            {
                return;
            }

            if (!requestBody.Content.TryGetValue("multipart/form-data", out var mediaType))
            {
                return;
            }

            var lst = context.FormParameterDescriptions.Where(t => Globals.IsSubclassOf(t.ModelMetadata.ContainerType, typeof(Enumeration<,>)));
            List<string> items = [];
            foreach (var item in lst)
            {
                _ = mediaType.Schema.Properties.Remove(item.Name);
                var name = item.Name.Split('.')[0];
                if (items.Contains(name))
                {
                    continue;
                }

                items.Add(name);
                var names = EnumerationExtensions.GetNames(item.ModelMetadata.ContainerType!)!
                            .Select(t => new OpenApiString(t)).ToList<IOpenApiAny>();
                mediaType.Schema.Properties.Add(name, new()
                {
                    Properties = null,
                    AllOf = null,
                    Enum = names,
                    Type = "string"
                });
            }
        }
    }
}
