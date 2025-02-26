namespace GamaEdtech.Common.Identity.DataProtection
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using NUlid;

    [Table(nameof(DataProtectionKey))]
    public class DataProtectionKey : IEntity<DataProtectionKey, Ulid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Ulid)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Ulid Id { get; set; }

        [Column(nameof(FriendlyName), DataType.UnicodeString)]
        [StringLength(50)]
        public string? FriendlyName { get; set; }

        [Column(nameof(Xml), DataType.UnicodeMaxString)]
        public string? Xml { get; set; }

        public void Configure(EntityTypeBuilder<DataProtectionKey> builder)
        {
        }
    }
}
