namespace GamaEdtech.Common.DataAnnotation.Schema
{
    using GamaEdtech.Common.Data;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ForeignKeyAttribute(string name) : System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute(DbProviderFactories.GetFactory.GetObjectName(name))
    {
    }
}
