namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using GamaEdtech.Common.TimeZone;

    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class TimeZoneIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var timeZoneId = value?.ToString();
            if (string.IsNullOrEmpty(timeZoneId))
            {
                return ValidationResult.Success;
            }

            var timeZones = validationContext.GetRequiredService<ITimeZoneProvider>().GetTimeZones();
            return timeZones?.Any(t => t.Id!.Equals(timeZoneId, StringComparison.Ordinal)) == true ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
