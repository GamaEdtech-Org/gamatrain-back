namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;

    using GamaEdtech.Common.Core;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NationalIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString())
|| (value is IEnumerable<string> lst
                ? lst.All(t => string.IsNullOrEmpty(t) || Globals.ValidateNationalId(t))
                : Globals.ValidateNationalId(value.ToString()!));
    }
}
