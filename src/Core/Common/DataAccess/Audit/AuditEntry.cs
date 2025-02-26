namespace GamaEdtech.Common.DataAccess.Audit
{
    using System.Collections.Generic;

    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using NUlid;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;

    [Table(nameof(AuditEntry))]
    public class AuditEntry : IEntity<AuditEntry, Ulid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Ulid)]
        public Ulid Id { get; set; }

        [Column(nameof(AuditId), DataType.Ulid)]
        public Ulid AuditId { get; set; }

        public Audit? Audit { get; set; }

        [Column(nameof(AuditType), DataType.Byte)]
        [Required]
        public AuditType? AuditType { get; set; }

        [Column(nameof(EntityType), DataType.Int)]
        public int EntityType { get; set; }

        [Column(nameof(IdentifierId), DataType.String)]
        [StringLength(100)]
        public string? IdentifierId { get; set; }

        public IList<AuditEntryProperty>? AuditEntryProperties { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntry> builder) => builder.OwnEnumeration<AuditEntry, AuditType, byte>(t => t.AuditType);
    }
}
