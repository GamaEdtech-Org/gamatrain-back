namespace GamaEdtech.Presentation.ViewModel.Location
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ManageLocationRequestViewModel
    {
        [Display]
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Display]
        [StringLength(100)]
        public string? LocalTitle { get; set; }

        [Display]
        [Required]
        [StringLength(50)]
        public string? Code { get; set; }

        [Display]
        public int? ParentId { get; set; }

        [Display]
        public int Latitude { get; set; }

        [Display]
        public int Longitude { get; set; }
    }
}
