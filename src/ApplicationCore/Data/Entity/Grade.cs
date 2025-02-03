namespace GamaEdtech.Backend.Data.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;

    using GamaEdtech.Backend.Data.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(Grade))]
    public class Grade : VersionableEntity<ApplicationUser, int, int?>, IEntity<Grade, int>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Int)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Column(nameof(Title), DataType.UnicodeString)]
        [StringLength(50)]
        [Required]
        public string? Title { get; set; }

        [Column(nameof(Description), DataType.UnicodeString)]
        [StringLength(100)]
        public string? Description { get; set; }

        [Column(nameof(Icon), DataType.UnicodeMaxString)]
        public string? Icon { get; set; }

        [Column(nameof(BoardId), DataType.Int)]
        public int BoardId { get; set; }
        public Board? Board { get; set; }

        public virtual ICollection<Subject>? Subjects { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Grade> builder)
        {
        }
    }
}
