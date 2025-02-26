namespace GamaEdtech.Common.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class AuditIgnoreAttribute : Attribute
    {
    }
}
