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

        [Column(nameof(ClassesQualityRate), TypeName = "float")]
        [Required]
        public double ClassesQualityRate { get; set; }

        [Column(nameof(EducationRate), TypeName = "float")]
        [Required]
        public double EducationRate { get; set; }

        [Column(nameof(ITTrainingRate), TypeName = "float")]
        [Required]
        public double ITTrainingRate { get; set; }

        [Column(nameof(SafetyAndHappinessRate), TypeName = "float")]
        [Required]
        public double SafetyAndHappinessRate { get; set; }

        [Column(nameof(BehaviorRate), TypeName = "float")]
        [Required]
        public double BehaviorRate { get; set; }

        [Column(nameof(TuitionRatioRate), TypeName = "float")]
        [Required]
        public double TuitionRatioRate { get; set; }

        [Column(nameof(FacilitiesRate), TypeName = "float")]
        [Required]
        public double FacilitiesRate { get; set; }

        [Column(nameof(ArtisticActivitiesRate), TypeName = "float")]
        [Required]
        public double ArtisticActivitiesRate { get; set; }

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
            _ = builder.HasIndex(t => new { t.CreationUserId, t.SchoolId }).IsUnique(true);
            _ = builder.HasIndex(t => new { t.Status });
            _ = builder.OwnEnumeration<SchoolComment, Status, byte>(t => t.Status);
        }
    }
}
