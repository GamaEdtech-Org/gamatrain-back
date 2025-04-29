namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(PostTag))]
    public class PostTag : CreationableEntity<ApplicationUser, int>, IEntity<PostTag, long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(PostId), DataType.Long)]
        [Required]
        public long PostId { get; set; }
        public Post Post { get; set; }

        [Column(nameof(TagId), DataType.Long)]
        [Required]
        public long TagId { get; set; }
        public Tag Tag { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<PostTag> builder) => builder.HasIndex(t => new { t.PostId, t.TagId }).IsUnique(true);
    }
}
