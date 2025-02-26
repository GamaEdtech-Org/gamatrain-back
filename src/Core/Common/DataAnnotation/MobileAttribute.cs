namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;
    using GamaEdtech.Common.Core;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed partial class MobileAttribute : ValidationAttribute, IClientModelValidator
    {
        private const string Pattern = "^(09)[0-9]{2}[0-9]{7}$";
        private static readonly Regex Regex = MobileRegex();

        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString())
|| (value is IEnumerable<string> lst ? lst.All(t => string.IsNullOrEmpty(t) || Regex.IsMatch(t)) : Regex.IsMatch((string)value));

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", Pattern));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", Data.Error.FormatMessage(msg)));
        }

        [GeneratedRegex(Pattern, RegexOptions.Compiled)]
        private static partial Regex MobileRegex();
    }
}
