namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using DynamicExpresso;

    using GamaEdtech.Common.Core;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DaysDistanceAttribute : ValidationAttribute
    {
        public DaysDistanceAttribute(string otherProperty, int maxDistance, string? expression = null)
            : base(() => "ValidationError")
        {
            OtherProperty = otherProperty;
            MaxDistance = maxDistance;
            Expression = expression;

            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_DaysDistance);
        }

        public string OtherProperty { get; internal set; }

        public int MaxDistance { get; internal set; }

        public string? Expression { get; internal set; }

        public string? FormatErrorMessage(string name, string? otherName) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, otherName, MaxDistance);

        protected override ValidationResult? IsValid(object? value, [NotNull] ValidationContext validationContext)
        {
            var castValue = value as DateTime?;
            if (castValue is null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(Expression))
            {
                var properties = validationContext.ObjectType.GetProperties();
                var interpreter = new Interpreter();
                MaxDistance = interpreter.Eval<int>(Expression, [.. from info in properties
                                                                 where Expression.Contains(info.Name, StringComparison.InvariantCultureIgnoreCase)
                                                                 select
                                                                 new Parameter(info.Name, info.PropertyType, info.GetValue(validationContext.ObjectInstance, null))]);
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null) as DateTime?;
            if (otherValue is null)
            {
                return null;
            }

            var displayName = Globals.GetLocalizedDisplayName(validationContext.ObjectType.GetProperty(validationContext.MemberName!));
            var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);

            return (castValue.Value - otherValue.Value).TotalDays > MaxDistance
                ? new ValidationResult(FormatErrorMessage(displayName!, otherDisplayName))
                : null;
        }
    }
}
