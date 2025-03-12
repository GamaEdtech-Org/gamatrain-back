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

    [Table(nameof(Tag))]
    public class Tag : VersionableEntity<ApplicationUser, int, int?>, IEntity<Tag, int>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Int)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Column(nameof(Name), DataType.UnicodeString)]
        [StringLength(100)]
        [Required]
        public string? Name { get; set; }

        [Column(nameof(TagType), DataType.Byte)]
        [Required]
        public TagType? TagType { get; set; }

        [Column(nameof(Icon), DataType.UnicodeMaxString)]
        public string? Icon { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Tag> builder)
        {
            _ = builder.HasIndex(t => t.TagType);
            _ = builder.HasIndex(t => new { t.TagType, t.Name }).IsUnique(true);
            _ = builder.OwnEnumeration<Tag, TagType, byte>(t => t.TagType);
        }
    }
}
