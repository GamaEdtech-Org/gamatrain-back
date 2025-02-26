namespace GamaEdtech.Domain.Entity.Identity
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using System.Diagnostics.CodeAnalysis;

    [Table(nameof(ApplicationUserLogin))]
    public class ApplicationUserLogin : IdentityUserLogin<int>, IEntity<ApplicationUserLogin, int>
    {
        [StringLength(128)]
        [Required]
        [Column(nameof(LoginProvider), DataType.UnicodeString)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public override string LoginProvider { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [StringLength(128)]
        [Required]
        [Column(nameof(ProviderKey), DataType.UnicodeString)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public override string ProviderKey { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [StringLength(128)]
        [Column(nameof(ProviderDisplayName), DataType.UnicodeString)]
        public override string? ProviderDisplayName { get; set; }

        [Required]
        [Column(nameof(UserId), DataType.Int)]
        public override int UserId { get; set; }

        public required ApplicationUser User { get; set; }

        int IIdentifiable<int>.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Configure([NotNull] EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            _ = builder.HasIndex(t => t.UserId)
                .HasDatabaseName(DbProviderFactories.GetFactory.GetObjectName($"IX_{nameof(ApplicationUserLogin)}_{nameof(UserId)}"));

            _ = builder.HasOne(t => t.User)
                .WithMany(t => t.UserLogins)
                .HasForeignKey(t => t.UserId);

            _ = builder.HasKey(t => new { t.LoginProvider, t.ProviderKey });
        }
    }
}
