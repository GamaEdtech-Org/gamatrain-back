namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(SchoolBoard))]
    public class SchoolBoard : CreationableEntity<ApplicationUser, int>, IEntity<SchoolBoard, long>, ISchoolId
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(SchoolId), DataType.Long)]
        public long SchoolId { get; set; }
        public School School { get; set; }

        [Column(nameof(BoardId), DataType.Int)]
        public int BoardId { get; set; }
        public Board Board { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<SchoolBoard> builder) => builder.HasIndex(t => new { t.SchoolId, t.BoardId }).IsUnique(true);
    }
}
