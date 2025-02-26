namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EmailAddressAttribute : ValidationAttribute, IClientModelValidator
    {
        public EmailAddressAttribute() => ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_EmailAddress);

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            var attribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            return value is IEnumerable<string> lst ? lst.All(t => string.IsNullOrEmpty(t) || attribute.IsValid(t)) : attribute.IsValid(value);
        }

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-email", Data.Error.FormatMessage(msg)));
        }
    }
}
