namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class LocationViewModel
    {
        [Display]
        [Required]
        public double? Latitude { get; set; }

        [Display]
        [Required]
        public double? Longitude { get; set; }

        [Display]
        [Required]
        public double? Radius { get; set; }
    }
}
