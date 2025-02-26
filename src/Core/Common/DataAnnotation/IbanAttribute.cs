namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IbanAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString())
|| (value is IEnumerable<string> lst ? lst.All(t => string.IsNullOrEmpty(t) || t.ValidateIban()) : value.ToString().ValidateIban());

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-iban", Data.Error.FormatMessage(msg)));
        }
    }
}
