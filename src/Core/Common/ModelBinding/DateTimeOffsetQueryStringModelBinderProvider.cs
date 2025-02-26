namespace GamaEdtech.Common.ModelBinding
{
    using System;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class DateTimeOffsetQueryStringModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var fullyQualifiedAssemblyName = context.Metadata.ModelType.FullName;
            if (fullyQualifiedAssemblyName is null)
            {
                return null;
            }

            var type = context.Metadata.ModelType.Assembly.GetType(fullyQualifiedAssemblyName, false);
            if (type is null)
            {
                return null;
            }

            var dateTimeOffsetType = typeof(DateTimeOffset);
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
            {
                dateTimeOffsetType = type.GenericTypeArguments[0];
            }

            return !type.IsSubclassOf(dateTimeOffsetType) ? null
                : Activator.CreateInstance(typeof(DateTimeOffsetQueryStringModelBinder).MakeGenericType(type)) as IModelBinder;
        }
    }
}
