namespace GamaEdtech.Common.DataAnnotation.Schema
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ColumnAttribute : System.ComponentModel.DataAnnotations.Schema.ColumnAttribute
    {
        public ColumnAttribute([NotNull] string name)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, pluralize: false))
        {
        }

        public ColumnAttribute([NotNull] string name, DataType dataType)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, pluralize: false))
        {
            TypeName = DbProviderFactories.GetFactory.GetColumnTypeName(dataType);
            DataType = dataType;
        }

        public DataType? DataType { get; }
    }
}
