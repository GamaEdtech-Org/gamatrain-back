namespace GamaEdtech.Common.DataAccess.Audit
{
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using NUlid;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;

    [Table(nameof(AuditEntryProperty))]
    public class AuditEntryProperty : IEntity<AuditEntryProperty, Ulid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Ulid)]
        public Ulid Id { get; set; }

        [Column(nameof(AuditEntryId), DataType.Ulid)]
        public Ulid AuditEntryId { get; set; }

        public AuditEntry? AuditEntry { get; set; }

        [StringLength(50)]
        [Required]
        [Column(nameof(PropertyName), DataType.String)]
        public string? PropertyName { get; set; }

        [Column(nameof(OldValue), DataType.UnicodeMaxString)]
        public string? OldValue { get; set; }

        [Column(nameof(NewValue), DataType.UnicodeMaxString)]
        public string? NewValue { get; set; }

        [NotMapped]
        public PropertyEntry? TemporaryProperty { get; set; }

        public void Configure(EntityTypeBuilder<AuditEntryProperty> builder)
        {
            // not working, go to IdentityEntityContext
        }
    }
}
