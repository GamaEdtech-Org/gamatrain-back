namespace GamaEdtech.Common.DataAnnotation.Schema
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ComplexTypeAttribute : System.ComponentModel.DataAnnotations.Schema.ComplexTypeAttribute
    {
        public ComplexTypeAttribute()
        {
        }
    }
}
