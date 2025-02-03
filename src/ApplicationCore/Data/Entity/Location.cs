namespace GamaEdtech.Backend.Data.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.Data;
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;

    using GamaEdtech.Backend.Data.Entity.Identity;
    using GamaEdtech.Backend.Data.Enumeration;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        [Column(nameof(Latitude), TypeName = "float")]
        public int Latitude { get; set; }

        [Column(nameof(Longitude), TypeName = "float")]
        public int Longitude { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Location> builder)
        {
            _ = builder.HasIndex(t => t.ParentId);
            _ = builder.HasIndex(t => t.LocationType);
            _ = builder.HasIndex(t => t.Code).IsUnique(true);
            _ = builder.OwnEnumeration<Location, LocationType, byte>(t => t.LocationType);
        }
    }
}
