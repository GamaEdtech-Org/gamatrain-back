namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CreditCardAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata!.ContainerType?.GetProperty(context.ModelMetadata!.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-creditcard", Data.Error.FormatMessage(msg)));
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return ValidationResult.Success;
            }

            var attribute = new System.ComponentModel.DataAnnotations.CreditCardAttribute();
            if (value is List<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || attribute.IsValid(t)) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
            }

            var valid = attribute.IsValid(value);
            return valid ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
