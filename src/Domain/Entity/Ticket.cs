namespace GamaEdtech.Domain.Entity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [Table(nameof(Ticket))]
    public class Ticket : IEntity<Ticket, long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(Sender), DataType.UnicodeString)]
        [StringLength(100)]
        public string? Sender { get; set; }

        [Column(nameof(CreationUserId), DataType.Int)]
        public int? CreationUserId { get; set; }
        public ApplicationUser? CreationUser { get; set; }

        [Column(nameof(CreationDate), DataType.DateTimeOffset)]
        [Required]
        public DateTimeOffset CreationDate { get; set; }

        [Column(nameof(Email), DataType.UnicodeString)]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column(nameof(Subject), DataType.UnicodeString)]
        [StringLength(500)]
        public string? Subject { get; set; }

        [Column(nameof(Body), DataType.UnicodeMaxString)]
        [Required]
        public string? Body { get; set; }

        [Column(nameof(IsRead), DataType.Boolean)]
        public bool IsRead { get; set; }

        [Column(nameof(IsReadByAdmin), DataType.Boolean)]
        public bool IsReadByAdmin { get; set; }

        [Column(nameof(ParentId), DataType.Long)]
        public long? ParentId { get; set; }
        public Ticket? Parent { get; set; }

        [Column(nameof(FileId), DataType.String)]
        [StringLength(100)]
        public string? FileId { get; set; }

        public void Configure([NotNull] EntityTypeBuilder<Ticket> builder) => _ = builder.HasIndex(t => t.Email);
    }
}
