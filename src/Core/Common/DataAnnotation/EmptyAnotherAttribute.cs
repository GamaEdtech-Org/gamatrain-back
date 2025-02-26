namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Core;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EmptyAnotherAttribute : ValidationAttribute
    {
        public EmptyAnotherAttribute([NotNull] string otherProperty)
            : base(() => "ValidationError")
        {
            ArgumentNullException.ThrowIfNull(otherProperty);

            OtherProperty = otherProperty;

            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_EmptyAnother);
        }

        public string OtherProperty { get; }

        protected override ValidationResult? IsValid(object? value, [NotNull] ValidationContext validationContext)
        {
            if (value is null)
            {
                return null;
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);
            if (otherValue is not null)
            {
                var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);
                var emptyAnother = Resources.GlobalResource.Validation_EmptyAnother;
                return new ValidationResult(string.Format(emptyAnother, otherDisplayName));
            }

            return null;
        }
    }
}
