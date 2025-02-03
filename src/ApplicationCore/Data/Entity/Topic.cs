namespace GamaEdtech.Backend.Data.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;

    using GamaEdtech.Backend.Data.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(Topic))]
    public class Topic : VersionableEntity<ApplicationUser, int, int?>, IEntity<Topic, int>
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

        [Column(nameof(Order), DataType.Int)]
        public int Order { get; set; }

        public virtual ICollection<Subject>? Subjects { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Topic> builder)
        {
        }
    }
}
