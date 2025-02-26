namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PhoneAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            var attribute = new System.ComponentModel.DataAnnotations.PhoneAttribute();
            return value is IEnumerable<string> lst ? lst.All(t => string.IsNullOrEmpty(t) || attribute.IsValid(t)) : attribute.IsValid(value);
        }

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-phone", Data.Error.FormatMessage(msg)));
        }
    }
}
