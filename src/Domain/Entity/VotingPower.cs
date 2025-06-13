namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(VotingPower))]
    public class VotingPower : VersionableEntity<ApplicationUser, int, int?>, IEntity<VotingPower, long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(ProposalId), DataType.UnicodeString)]
        [Required]
        [StringLength(512)]
        public string? ProposalId { get; set; }

        [Column(nameof(WalletAddress), DataType.UnicodeString)]
        [Required]
        [StringLength(512)]
        public string? WalletAddress { get; set; }

        [Column(nameof(Amount), DataType.Decimal)]
        [Required]
        public decimal Amount { get; set; }

        [Column(nameof(TokenAccount), DataType.UnicodeString)]
        [Required]
        [StringLength(512)]
        public string? TokenAccount { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<VotingPower> builder)
        {
            _ = builder.HasIndex(t => t.ProposalId);
            _ = builder.HasIndex(t => t.WalletAddress);
            _ = builder.Property(t => t.Amount).HasPrecision(36, 18);
        }
    }
}
