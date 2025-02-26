namespace GamaEdtech.Common.DataAccess.Audit
{
    using System;
    using System.Collections.Generic;

    using NUlid;

    public class AuditDto
    {
        public Ulid Id { get; set; }

        public string? User { get; set; }

        public DateTimeOffset Date { get; set; }

        public string? UserAgent { get; set; }

        public string? IpAddress { get; set; }

        public ICollection<AuditEntryDto>? AuditEntries { get; set; }
    }
}
