namespace GamaEdtech.Common.DataAccess.Audit
{
    using System;
    using System.Collections.Generic;

    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using NUlid;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;

    [Table(nameof(Audit))]
    public class Audit : IEntity<Audit, Ulid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Ulid)]
        public Ulid Id { get; set; }

        [Column(nameof(UserId), DataType.String)]
        [StringLength(50)]
        public string? UserId { get; set; }

        [Column(nameof(UserName), DataType.UnicodeString)]
        [StringLength(50)]
        public string? UserName { get; set; }

        [Column(nameof(Date), DataType.DateTimeOffset)]
        public DateTimeOffset Date { get; set; }

        [StringLength(1000)]
        [Column(nameof(UserAgent), DataType.UnicodeString)]
        public string? UserAgent { get; set; }

        [StringLength(50)]
        [Column(nameof(IpAddress), DataType.String)]
        public string? IpAddress { get; set; }

        public IList<AuditEntry>? AuditEntries { get; set; }

        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            // not working, go to IdentityEntityContext
        }
    }
}
