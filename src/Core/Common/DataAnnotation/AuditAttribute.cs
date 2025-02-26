namespace GamaEdtech.Common.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AuditAttribute(int entityType) : Attribute
    {
        public int EntityType { get; } = entityType;
    }
}
