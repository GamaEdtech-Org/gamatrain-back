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

    using NetTopologySuite.Geometries;

    [Table(nameof(Location))]
    public class Location : VersionableEntity<ApplicationUser, int, int?>, IEntity<Location, int>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Int)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Column(nameof(Title), DataType.UnicodeString)]
        [StringLength(100)]
        [Required]
        public string? Title { get; set; }

        [Column(nameof(LocalTitle), DataType.UnicodeString)]
        [StringLength(100)]
        public string? LocalTitle { get; set; }

        [Column(nameof(Code), DataType.String)]
        [StringLength(50)]
        public string? Code { get; set; }

        [Column(nameof(Type), DataType.Byte)]
        [Required]
        public LocationType? LocationType { get; set; }

        [Column(nameof(ParentId), DataType.Int)]
        public int? ParentId { get; set; }
        public Location? Parent { get; set; }

        [Column(nameof(Coordinates), TypeName = "geometry")]
        public Point? Coordinates { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Location> builder)
        {
            _ = builder.HasIndex(t => t.ParentId);
            _ = builder.HasIndex(t => t.LocationType);
            _ = builder.HasIndex(t => t.Code).IsUnique(true);
            _ = builder.OwnEnumeration<Location, LocationType, byte>(t => t.LocationType);
        }
    }
}
