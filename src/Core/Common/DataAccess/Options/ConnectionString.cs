namespace GamaEdtech.Common.DataAccess.Options
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GamaEdtech.Common.Data;

    public class ConnectionString(DbProviderType providerType, string? host, string? dbIdentifier, string? user, string? password, ushort? port = null, Dictionary<string, string>? parameters = null)
    {
        public DbProviderType ProviderType { get; set; } = providerType;

        public string? Host { get; set; } = host;

        public ushort? Port { get; set; } = port;

        public string? DbIdentifier { get; set; } = dbIdentifier;

        public string? User { get; set; } = user;

        public string? Password { get; set; } = password;

        public Dictionary<string, string>? Parameters { get; set; } = parameters;

        public override string? ToString()
        {
            var sb = new StringBuilder();
            switch (ProviderType)
            {
                case DbProviderType.SqlServer:
                    _ = sb.Append($"Server={Host};Database={DbIdentifier};User Id={User};Password={Password};");

                    if (Port.HasValue)
                    {
                        _ = sb.Append($"Port={Port.Value};");
                    }

                    break;
                case DbProviderType.DevartOracle:
                    if (!Port.HasValue)
                    {
                        Port = 1521;
                    }

                    _ = sb.Append($"User Id={User};Password={Password};Direct=true;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={Host})(PORT={Port}))(CONNECT_DATA=(SID={DbIdentifier})))");

                    break;
                default:
                    return string.Empty;
            }

            if (Parameters?.Count > 0)
            {
                _ = sb.AppendJoin(";", Parameters.Keys.Select(t => $"{t}={Parameters[t]}"));
            }

            return sb.ToString();
        }
    }
}
