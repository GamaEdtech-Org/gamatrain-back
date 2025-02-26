namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CultureAttribute : ValidationAttribute, IClientModelValidator
    {
        public CultureAttribute() => ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Culture);

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            return value is IEnumerable<string> lst
                ? lst.All(t => string.IsNullOrEmpty(t) || cultures.Exists(c => c.Name.Equals(t, StringComparison.OrdinalIgnoreCase)))
                : cultures.Exists(t => t.Name == value.ToString());
        }

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", "^[a-z]{2}(-[A-Z]{2})*"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata!.ContainerType?.GetProperty(context.ModelMetadata!.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", Data.Error.FormatMessage(msg)));
        }
    }
}
