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

    [Table(nameof(Reaction))]
    public class Reaction : CreationableEntity<ApplicationUser, int>, IEntity<Reaction, long>, ICategoryType, IIdentifierId
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(CategoryType), DataType.Byte)]
        [Required]
        public CategoryType CategoryType { get; set; }

        [Column(nameof(IdentifierId), DataType.Long)]
        [Required]
        public long? IdentifierId { get; set; }

        [Column(nameof(IsLike), DataType.Boolean)]
        [Required]
        public bool IsLike { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Reaction> builder)
        {
            _ = builder.OwnEnumeration<Reaction, CategoryType, byte>(t => t.CategoryType);
            _ = builder.HasIndex(t => new
            {
                t.CategoryType,
                t.IdentifierId,
                t.CreationUserId,
            }).IsUnique(true);
        }
    }
}
