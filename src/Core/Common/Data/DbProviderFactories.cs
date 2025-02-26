namespace GamaEdtech.Common.Data
{
    using GamaEdtech.Common.Core;

    public static class DbProviderFactories
    {
        private static DbProviderFactory? factory;

        public static DbProviderFactory GetFactory
        {
            get
            {
                if (factory is not null)
                {
                    return factory;
                }

                factory = Globals.ProviderType switch
                {
                    DbProviderType.SqlServer => new SqlServerProvider(),
                    _ => throw new NotSupportedException(Globals.ProviderType.ToString()),
                };
                return factory;
            }
        }
    }
}
