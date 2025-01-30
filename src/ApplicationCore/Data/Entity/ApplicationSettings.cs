namespace GamaEdtech.Backend.Data.Entity
{
    using GamaEdtech.Backend.Common;
    using GamaEdtech.Backend.Data.Entity.Identity;

    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Entities;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.DataAnnotation.Schema;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using System.Diagnostics.CodeAnalysis;

    [Table(nameof(ApplicationSettings))]
    [Audit((int)Constants.EntityType.ApplicationSetting)]
    public class ApplicationSettings : VersionableEntity<ApplicationUser, int, int?>, IEntity<ApplicationSettings, string>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.String)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(50)]
        [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Column(nameof(Value), DataType.UnicodeMaxString)]
        [Required]
        public string? Value { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<ApplicationSettings> builder)
        {
        }
    }
}
