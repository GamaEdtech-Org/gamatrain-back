namespace GamaEdtech.Common.ModelBinding
{
    using System;

    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EnumerationQueryStringModelBinderProvider : IModelBinderProvider
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

            var enumType = context.Metadata.ModelType.Assembly.GetType(fullyQualifiedAssemblyName, false);
            if (enumType is null)
            {
                return null;
            }

            var typeOfEnumeration = typeof(Enumeration<>);
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(enumType) && enumType.IsGenericType)
            {
                typeOfEnumeration = enumType.GenericTypeArguments[0];
            }

            return !enumType.IsSubclassOf(typeOfEnumeration)
                ? null
                : Activator.CreateInstance(typeof(EnumerationQueryStringModelBinder<,>).MakeGenericType(enumType)) as IModelBinder;
        }
    }
}
