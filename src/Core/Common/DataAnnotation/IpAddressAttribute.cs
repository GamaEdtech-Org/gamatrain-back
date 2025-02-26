namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IpAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString())
|| (value is IEnumerable<string> lst ? lst.All(t => string.IsNullOrEmpty(t) || Validate(t)) : Validate(value.ToString()));

        private static bool Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            var splitValues = value.Split('.');
            return splitValues.Length == 4 && splitValues.All(r => byte.TryParse(r, out var _));
        }
    }
}
