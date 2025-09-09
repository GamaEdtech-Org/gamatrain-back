namespace GamaEdtech.Presentation.ViewModel.Identity
{
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
        public IFormFile? AvatarFile { get; set; }
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
    }
}
