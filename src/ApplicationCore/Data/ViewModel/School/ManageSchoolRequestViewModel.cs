namespace GamaEdtech.Backend.Data.ViewModel.School
{
    using Farsica.Framework.DataAnnotation;

    using GamaEdtech.Backend.Data.Enumeration;

    public sealed class ManageSchoolRequestViewModel
    {
        [Display]
        [Required]
        public string? Name { get; set; }

        [Display]
        [Required]
        public string? LocalName { get; set; }

        [Display]
        [Required]
        public SchoolType? SchoolType { get; set; }

        [Display]
        [Required]
        public int? StateId { get; set; }

        [Display]
        [Required]
        public string? ZipCode { get; set; }

        [Display]
        [Required]
        public string? Address { get; set; }

        [Display]
        [Required]
        public double? Latitude { get; set; }

        [Display]
        [Required]
        public double? Longitude { get; set; }
    }
}
