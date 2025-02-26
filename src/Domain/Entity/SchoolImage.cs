namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using NUlid;

    [Table(nameof(SchoolImage))]
    public class SchoolImage : VersionableEntity<ApplicationUser, int, int?>, IEntity<SchoolImage, long>, ISchoolId, IStatus
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(SchoolId), DataType.Int)]
        [Required]
        public int SchoolId { get; set; }
        public School? School { get; set; }

        [Column(nameof(FileId), DataType.Ulid)]
        [Required]
        public Ulid FileId { get; set; }

        [Column(nameof(Status), DataType.Byte)]
        [Required]
        public Status? Status { get; set; }

        [Column(nameof(FileType), DataType.Byte)]
        [Required]
        public FileType? FileType { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<SchoolImage> builder)
        {
            _ = builder.OwnEnumeration<SchoolImage, Status, byte>(t => t.Status);
            _ = builder.OwnEnumeration<SchoolImage, FileType, byte>(t => t.FileType);
            _ = builder.HasIndex(t => t.Status);
        }
    }
}
