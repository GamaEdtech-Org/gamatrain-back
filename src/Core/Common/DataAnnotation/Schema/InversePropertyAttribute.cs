namespace GamaEdtech.Common.DataAnnotation.Schema
{
    using GamaEdtech.Common.Data;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class InversePropertyAttribute(string property) : System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute(DbProviderFactories.GetFactory.GetObjectName(property))
    {
    }
}
