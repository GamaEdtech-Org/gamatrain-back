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

    [Table(nameof(SchoolImage))]
    public class SchoolImage : VersionableEntity<ApplicationUser, int, int?>, IEntity<SchoolImage, long>, ISchoolId
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(SchoolId), DataType.Long)]
        [Required]
        public long SchoolId { get; set; }
        public School? School { get; set; }

        [Column(nameof(FileId), DataType.String)]
        [Required]
        [StringLength(100)]
        public string? FileId { get; set; }

        [Column(nameof(FileType), DataType.Byte)]
        [Required]
        public FileType? FileType { get; set; }

        [Column(nameof(TagId), DataType.Long)]
        public long? TagId { get; set; }
        public Tag? Tag { get; set; }

        [Column(nameof(ContributionId), DataType.Long)]
        public long? ContributionId { get; set; }
        public Contribution? Contribution { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<SchoolImage> builder) => _ = builder.OwnEnumeration<SchoolImage, FileType, byte>(t => t.FileType);
    }
}
