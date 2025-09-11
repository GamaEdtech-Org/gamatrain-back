namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Http;

    public sealed class ProfileSettingsRequestViewModel
    {
        [Display]
        public int? CityId { get; set; }
        [Display]
        public long? SchoolId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public int? Section { get; set; }
        public int? Grade { get; set; }

        [Display]
        [FileSize(1024 * 1024 * 2)]//2MB
        [FileExtensions(Constants.ValidImageExtensions)]
        public IFormFile? Avatar { get; set; }
        public string? UserName { get; set; }
    }
}
