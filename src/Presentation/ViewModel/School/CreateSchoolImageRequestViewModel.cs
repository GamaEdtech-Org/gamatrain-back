namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;

    public sealed class CreateSchoolImageRequestViewModel
    {
        [Display]
        [Required]
        public IFormFile? File { get; set; }

        [Display]
        public long? TagId { get; set; }

        [Display]
        [Required]
        public FileType? FileType { get; set; }

        [Display]
        public bool IsDefault { get; set; }
    }
}
