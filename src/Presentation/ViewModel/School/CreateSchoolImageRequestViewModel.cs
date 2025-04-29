namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public sealed class CreateSchoolImageRequestViewModel
    {
        [Display]
        [Required]
        [FromForm]
        public IFormFile? File { get; set; }

        [Display]
        [FromForm]
        public long? TagId { get; set; }
    }
}
