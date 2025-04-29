namespace GamaEdtech.Common.ModelBinding
{
    using System;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EnumerationQueryStringModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var modelType = context.Metadata.ModelType;

            if (modelType.HasElementType)
            {
                modelType = modelType.GetElementType()!;
            }
            else if (modelType.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(modelType))
            {
                modelType = modelType.GenericTypeArguments[0];
            }

            return Globals.IsSubclassOf(modelType, typeof(Enumeration<,>))
                ? new EnumerationQueryStringModelBinder(context.Metadata.ModelType)
                : null;
        }
    }
}
