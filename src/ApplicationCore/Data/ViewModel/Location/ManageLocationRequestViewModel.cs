namespace GamaEdtech.Backend.Data.ViewModel.Location
{
    using Farsica.Framework.DataAnnotation;

    public sealed class ManageLocationRequestViewModel
    {
        [Display]
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Display]
        [Required]
        [StringLength(50)]
        public string? Code { get; set; }

        [Display]
        public int? ParentId { get; set; }
    }
}
