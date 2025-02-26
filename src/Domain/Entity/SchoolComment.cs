namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(SchoolComment))]
    public class SchoolComment : VersionableEntity<ApplicationUser, int, int?>, IEntity<SchoolComment, long>, ISchoolId, IStatus
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

        [Column(nameof(Comment), DataType.UnicodeMaxString)]
        public string? Comment { get; set; }

        [Column(nameof(ClassesQualityRate), DataType.Byte)]
        [Required]
        public byte ClassesQualityRate { get; set; }

        [Column(nameof(EducationRate), DataType.Byte)]
        [Required]
        public byte EducationRate { get; set; }

        [Column(nameof(ITTrainingRate), DataType.Byte)]
        [Required]
        public byte ITTrainingRate { get; set; }

        [Column(nameof(SafetyAndHappinessRate), DataType.Byte)]
        [Required]
        public byte SafetyAndHappinessRate { get; set; }

        [Column(nameof(BehaviorRate), DataType.Byte)]
        [Required]
        public byte BehaviorRate { get; set; }

        [Column(nameof(TuitionRatioRate), DataType.Byte)]
        [Required]
        public byte TuitionRatioRate { get; set; }

        [Column(nameof(FacilitiesRate), DataType.Byte)]
        [Required]
        public byte FacilitiesRate { get; set; }

        [Column(nameof(ArtisticActivitiesRate), DataType.Byte)]
        [Required]
        public byte ArtisticActivitiesRate { get; set; }

        [Column(nameof(LikeCount), DataType.Int)]
        [Required]
        public int LikeCount { get; set; }

        [Column(nameof(DislikeCount), DataType.Int)]
        [Required]
        public int DislikeCount { get; set; }

        [Column(nameof(AverageRate), DataType.Decimal)]
        [Required]
        [Precision(3, 2)]
        public double AverageRate { get; set; }

        [Column(nameof(Status), DataType.Byte)]
        [Required]
        public Status? Status { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<SchoolComment> builder)
        {
            _ = builder.HasOne(t => t.School).WithMany(t => t.Comments).HasForeignKey(t => t.SchoolId).OnDelete(DeleteBehavior.NoAction);
            _ = builder.HasOne(t => t.CreationUser).WithMany().HasForeignKey(t => t.CreationUserId).OnDelete(DeleteBehavior.NoAction);
            _ = builder.HasIndex(t => new { t.CreationUserId, t.SchoolId }).IsUnique(true);
            _ = builder.HasIndex(t => new { t.Status });
        }
    }
}
