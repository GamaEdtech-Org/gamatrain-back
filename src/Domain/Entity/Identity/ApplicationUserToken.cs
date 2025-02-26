namespace GamaEdtech.Domain.Entity.Identity
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using System.Diagnostics.CodeAnalysis;

    [Table(nameof(ApplicationUserToken))]
    public class ApplicationUserToken : IdentityUserToken<int>, IEntity<ApplicationUserToken, int>
    {
        [Required]
        [Column(nameof(UserId), DataType.Int)]
        public override int UserId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Column(nameof(LoginProvider), DataType.UnicodeString)]
        [StringLength(128)]
        [Required]
        public override string LoginProvider { get; set; }

        [Column(nameof(Name), DataType.UnicodeString)]
        [StringLength(128)]
        [Required]
        public override string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Column(nameof(Value), DataType.UnicodeString)]
        [StringLength(2500)]
        public override string? Value { get; set; }

        public ApplicationUser? User { get; set; }

        int IIdentifiable<int>.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Configure([NotNull] EntityTypeBuilder<ApplicationUserToken> builder)
        {
            _ = builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            _ = builder.HasOne(t => t.User)
                .WithMany(t => t.UserTokens)
                .HasForeignKey(t => t.UserId);
        }
    }
}