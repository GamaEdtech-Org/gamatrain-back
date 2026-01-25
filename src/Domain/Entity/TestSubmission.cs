namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(TestSubmission))]
    public class TestSubmission : IEntity<TestSubmission, long>, ICreationDate, IUserId<int>
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

        [Column(nameof(TestId), DataType.Long)]
        [Required]
        public long TestId { get; set; }

        [Column(nameof(SubmissionId), DataType.Int)]
        [Required]
        public int SubmissionId { get; set; }

        [Column(nameof(CreationDate), DataType.DateTimeOffset)]
        [Required]
        public DateTimeOffset CreationDate { get; set; }

        [Column(nameof(IsCorrect), DataType.Boolean)]
        [Required]
        public bool IsCorrect { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<TestSubmission> builder) => _ = builder.HasIndex(t => new { t.UserId, t.TestId }).IsUnique();
    }
}
