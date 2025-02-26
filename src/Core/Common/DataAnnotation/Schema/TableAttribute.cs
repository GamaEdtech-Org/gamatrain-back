namespace GamaEdtech.Common.DataAnnotation.Schema
{
    using GamaEdtech.Common.Data;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TableAttribute(string name, string? prefix = null, bool pluralize = true) : System.ComponentModel.DataAnnotations.Schema.TableAttribute(DbProviderFactories.GetFactory.GetObjectName(name, prefix, pluralize))
    {
        public string? Prefix { get; } = prefix;

        public bool Pluralize { get; } = pluralize;
    }
}
