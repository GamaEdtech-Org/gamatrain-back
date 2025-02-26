namespace GamaEdtech.Common.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AreaAttribute(string areaName, string? displayName = null) : Microsoft.AspNetCore.Mvc.AreaAttribute(areaName)
    {
        public string AreaName { get; } = areaName;

        public string? DisplayName { get; } = displayName;
    }
}
