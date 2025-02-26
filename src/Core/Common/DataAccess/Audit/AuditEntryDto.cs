namespace GamaEdtech.Common.DataAccess.Audit
{
    using System.Collections.Generic;

    using NUlid;

    public class AuditEntryDto
    {
        public Ulid Id { get; set; }

        public Ulid AuditId { get; set; }

        public AuditType? AuditType { get; set; }

        public int EntityType { get; set; }

        public string? IdentifierId { get; set; }

        public ICollection<AuditEntryPropertyDto>? AuditEntryProperties { get; set; }
    }
}
