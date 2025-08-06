namespace GamaEdtech.Domain.Entity
{

    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(ReferralUser))]
    public class ReferralUser : VersionableEntity<ApplicationUser, int, int?>, IEntity<ReferralUser, int>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Int)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [Column(nameof(Name), DataType.UnicodeString)]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column(nameof(Family), DataType.UnicodeString)]
        [StringLength(50)]
        public string Family { get; set; }
        [Required]
        [Column(nameof(ReferralId), DataType.UnicodeString)]
        [StringLength(10)]
        public string ReferralId { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<ReferralUser> builder) => _ = builder.HasIndex(t => t.ReferralId).IsUnique(true);
    }
}
