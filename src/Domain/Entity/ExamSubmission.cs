namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(ExamSubmission))]
    public class ExamSubmission : IEntity<ExamSubmission, long>, ICreationDate, IUserId<int>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(UserId), DataType.Int)]
        [Required]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Column(nameof(ExamId), DataType.Long)]
        [Required]
        public long ExamId { get; set; }

        [Column(nameof(CreationDate), DataType.DateTimeOffset)]
        [Required]
        public DateTimeOffset CreationDate { get; set; }

        [Column(nameof(Valid), DataType.Int)]
        [Required]
        public int Valid { get; set; }

        [Column(nameof(Invalid), DataType.Int)]
        [Required]
        public int Invalid { get; set; }

        [Column(nameof(NoAnswer), DataType.Int)]
        [Required]
        public int NoAnswer { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<ExamSubmission> builder) => _ = builder.HasIndex(t => new { t.UserId, t.ExamId }).IsUnique();
    }
}
