namespace GamaEdtech.Common.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public abstract class DbProviderFactory
    {
        protected abstract IReadOnlyDictionary<DataType, string> Mapper { get; }

        public string GetObjectName([NotNull] string name, string? prefix = null, bool pluralize = true)
        {
            if (pluralize)
            {
                name = PluralizeService.Core.PluralizationProvider.Pluralize(name);
            }

            return GetObjectNameInternal(name, prefix);
        }

        public string GetColumnTypeName(DataType dataType) => Mapper[dataType];

        protected abstract string GetObjectNameInternal(string name, string? prefix = null);
    }
}
