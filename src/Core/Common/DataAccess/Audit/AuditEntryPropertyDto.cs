namespace GamaEdtech.Common.DataAccess.Audit
{
    using NUlid;

    public class AuditEntryPropertyDto
    {
        public Ulid Id { get; set; }

        public Ulid AuditEntryId { get; set; }

        public string? PropertyName { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }
    }
}
