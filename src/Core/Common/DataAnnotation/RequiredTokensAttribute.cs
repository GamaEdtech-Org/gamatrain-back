namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredTokensAttribute : ValidationAttribute
    {
        public RequiredTokensAttribute([NotNull] params string[] tokens)
            : base(() => "ValidationError")
        {
            ArgumentNullException.ThrowIfNull(tokens);

            Tokens = tokens;
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_RequiredAnother);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var str = value?.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return ValidationResult.Success;
            }

            var firstInvalidParameter = Tokens.FirstOrDefault(t => !str.Contains(t, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(firstInvalidParameter))
            {
                return ValidationResult.Success;
            }

            var requiredAnother = Resources.GlobalResource.Validation_RequiredAnother;
            return new ValidationResult(string.Format(requiredAnother, firstInvalidParameter));
        }

        public string[] Tokens { get; }
    }
}
