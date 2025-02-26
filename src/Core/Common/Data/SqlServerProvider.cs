namespace GamaEdtech.Common.Data
{
    using System.Collections.Generic;

    internal sealed class SqlServerProvider : DbProviderFactory
    {
        protected override IReadOnlyDictionary<DataType, string> Mapper => new Dictionary<DataType, string>
        {
            { DataType.Boolean, "bit" },
            { DataType.Byte, "tinyint" },
            { DataType.Short, "smallint" },
            { DataType.Int, "int" },
            { DataType.Long, "bigint" },
            { DataType.Decimal, "numeric" },
            { DataType.String, "varchar" },
            { DataType.UnicodeString, "nvarchar" },
            { DataType.MaxString, "varchar(max)" },
            { DataType.UnicodeMaxString, "nvarchar(max)" },
            { DataType.Char, "char" },
            { DataType.UnicodeChar, "nchar" },
            { DataType.Date, "date" },
            { DataType.Time, "time" },
            { DataType.DateTime, "datetime" },
            { DataType.DateTimeOffset, "datetimeoffset" },
            { DataType.Guid, "uniqueidentifier" },
            { DataType.File, "varbinary(max)" },
            { DataType.Ulid, "char(26)" },
        };

        protected override string GetObjectNameInternal(string name, string? prefix = null)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                name = $"{prefix}.{name}";
            }

            return name;
        }
    }
}
