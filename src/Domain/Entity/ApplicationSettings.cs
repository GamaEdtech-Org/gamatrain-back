namespace GamaEdtech.Domain.Entity
{
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using System.Diagnostics.CodeAnalysis;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Domain.Entity.Identity;

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
