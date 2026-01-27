namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(SiteMap))]
    public class SiteMap : IEntity<SiteMap, long>, IIdentifierId
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(IdentifierId), DataType.Long)]
        [Required]
        public long? IdentifierId { get; set; }

        [Column(nameof(ItemType), DataType.Byte)]
        [Required]
        public ItemType ItemType { get; set; }

        [Column(nameof(Priority))]
        public double? Priority { get; set; }

        [Column(nameof(ChangeFrequency), DataType.Byte)]
        public ChangeFrequency? ChangeFrequency { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<SiteMap> builder)
        {
            _ = builder.OwnEnumeration<SiteMap, ItemType, byte>(t => t.ItemType);
            _ = builder.OwnEnumeration<SiteMap, ChangeFrequency, byte>(t => t.ChangeFrequency);

            _ = builder.HasIndex(t => new { t.ItemType, t.IdentifierId }).IsUnique(true);
        }
    }
}
