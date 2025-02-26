namespace GamaEdtech.Common.DataAccess.Audit
{
    using System;

    using NUlid;

    public class AuditListDto
    {
        public Ulid Id { get; set; }

        public string? User { get; set; }

        public DateTimeOffset Date { get; set; }

        public string? IpAddress { get; set; }
    }
}
