namespace GamaEdtech.Domain.Entity
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;
    using GamaEdtech.Domain.Entity.Identity;

    [Table(nameof(Video))]
    public class Video : VersionableEntity<ApplicationUser, int, int?>//, IEntity<Video, long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(nameof(Id), DataType.Long)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Column(nameof(YouTubeVideoId), DataType.UnicodeString)]
        [StringLength(50)]
        [Required]
        public string? YouTubeVideoId { get; set; }

        [Column(nameof(Title), DataType.UnicodeString)]
        [StringLength(50)]
        [Required]
        public string? Title { get; set; }

        [Column(nameof(Description), DataType.UnicodeString)]
        [StringLength(100)]
        public string? Description { get; set; }

        [Column(nameof(Icon), DataType.UnicodeMaxString)]
        public string? Icon { get; set; }
    }
}
