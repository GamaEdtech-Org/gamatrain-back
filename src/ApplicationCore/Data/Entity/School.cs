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

    using NetTopologySuite.Geometries;

    [Table(nameof(School))]
    public class School : VersionableEntity<ApplicationUser, int, int?>, IEntity<School, int>
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

        [Column(nameof(Type), DataType.Byte)]
        [Required]
        public SchoolType? SchoolType { get; set; }

        [Column(nameof(StateId), DataType.Int)]
        [Required]
        public int StateId { get; set; }
        public Location? State { get; set; }

        [Column(nameof(ZipCode), DataType.String)]
        [StringLength(50)]
        [Required]
        public string? ZipCode { get; set; }

        [Column(nameof(Address), DataType.UnicodeString)]
        [StringLength(500)]
        [Required]
        public string? Address { get; set; }

        [Column(nameof(Location), TypeName = "geometry")]
        [Required]
        public Point? Location { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<School> builder) => _ = builder.OwnEnumeration<School, SchoolType, byte>(t => t.SchoolType);
    }
}
