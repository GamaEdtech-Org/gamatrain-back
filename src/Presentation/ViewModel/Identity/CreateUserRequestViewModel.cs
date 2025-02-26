namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Http;

    public sealed class CreateUserRequestViewModel
    {
        [Display]
        [Required]
        [StringLength(256)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public required string Username { get; set; }

        [Display]
        [Required]
        public required string Password { get; set; }

        [Display]
        [Required]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }

        [Display]
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Display]
        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Display]
        public string? FirstName { get; set; }

        [Display]
        public string? LastName { get; set; }

        [Display]
        [FileSize(1024 * 1024 * 2)]//2MB
        [FileExtensions(Constants.ValidImageExtensions)]
        public IFormFile? Avatar { get; set; }

        [Display]
        public IEnumerable<string>? AllowedIpAddressesList { get; set; }
    }
}
