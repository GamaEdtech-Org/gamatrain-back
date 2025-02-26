namespace GamaEdtech.Common.DataAccess.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class HasPrecisionAttribute(byte precision, byte scale) : Attribute
    {
        public byte Precision { get; } = precision;

        public byte Scale { get; } = scale;
    }
}
