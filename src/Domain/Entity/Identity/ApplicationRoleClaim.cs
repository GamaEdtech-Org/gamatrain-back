namespace GamaEdtech.Domain.Entity.Identity
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using System.Diagnostics.CodeAnalysis;

    [Table(nameof(ApplicationRoleClaim))]
    public class ApplicationRoleClaim : IdentityRoleClaim<int>, IEntity<ApplicationRoleClaim, int>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Int)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        [Column(nameof(RoleId), DataType.Int)]
        public override int RoleId { get; set; }

        [StringLength(128)]
        [Column(nameof(ClaimType), DataType.String)]
        [Required]
        public override string? ClaimType { get; set; }

        [StringLength(128)]
        [Column(nameof(ClaimValue), DataType.UnicodeString)]
        public override string? ClaimValue { get; set; }

        public required ApplicationRole Role { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            _ = builder.HasIndex(t => t.RoleId);
            _ = builder.HasOne(d => d.Role)
                .WithMany(p => p.RoleClaims)
                .HasForeignKey(d => d.RoleId);
        }
    }
}
