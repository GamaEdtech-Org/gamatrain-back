namespace GamaEdtech.Common.ModelBinding
{
    using System;

    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class FlagsEnumerationQueryStringModelBinderProvider : IModelBinderProvider
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

            var flagsEnumType = context.Metadata.ModelType.Assembly.GetType(fullyQualifiedAssemblyName, false);
            if (flagsEnumType is null)
            {
                return null;
            }

            var typeOfFlagsEnumeration = typeof(FlagsEnumeration<>);
            return !flagsEnumType.IsSubclassOf(typeOfFlagsEnumeration)
                ? null
                : Activator.CreateInstance(typeof(FlagsEnumerationQueryStringModelBinder<>).MakeGenericType(flagsEnumType)) as IModelBinder;
        }
    }
}
