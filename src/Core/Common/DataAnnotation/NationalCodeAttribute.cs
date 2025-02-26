namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;

    using GamaEdtech.Common.Core;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NationalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString())
|| (value is IEnumerable<string> lst
                ? lst.All(t => string.IsNullOrEmpty(t) || Globals.ValidateNationalCode(t))
                : Globals.ValidateNationalCode(value.ToString()!));
    }
}
