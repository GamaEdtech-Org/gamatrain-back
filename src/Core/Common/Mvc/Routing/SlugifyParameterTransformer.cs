namespace GamaEdtech.Common.Mvc.Routing
{
    using GamaEdtech.Common.Core;

    using Microsoft.AspNetCore.Routing;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value) => (value as string).Slugify();
    }
}
