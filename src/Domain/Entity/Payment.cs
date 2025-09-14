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

    [Table(nameof(Payment))]
    public class Payment : IEntity<Payment, long>, IUserId<int>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(UserId), DataType.Int)]
        [Required]
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Column(nameof(Amount), DataType.Decimal)]
        [Required]
        public decimal Amount { get; set; }

        [Column(nameof(Currency), DataType.Byte)]
        [Required]
        public Currency Currency { get; set; }

        [Column(nameof(Status), DataType.Byte)]
        [Required]
        public PaymentStatus Status { get; set; }

        [Column(nameof(CreationDate), DataType.DateTimeOffset)]
        [Required]
        public DateTimeOffset CreationDate { get; set; }

        [Column(nameof(VerifyDate), DataType.DateTimeOffset)]
        public DateTimeOffset? VerifyDate { get; set; }

        [Column(nameof(SourceWallet), DataType.UnicodeString)]
        [StringLength(200)]
        public string? SourceWallet { get; set; }

        [Column(nameof(Comment), DataType.UnicodeString)]
        [StringLength(100)]
        public string? Comment { get; set; }

        [Column(nameof(TransactionId), DataType.UnicodeString)]
        [StringLength(200)]
        public string? TransactionId { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Payment> builder)
        {
            _ = builder.Property(t => t.Amount).HasPrecision(36, 18);
            _ = builder.OwnEnumeration<Payment, Currency, byte>(t => t.Currency);
            _ = builder.OwnEnumeration<Payment, PaymentStatus, byte>(t => t.Status);
            _ = builder.HasIndex(t => t.TransactionId).IsUnique();
        }
    }
}
