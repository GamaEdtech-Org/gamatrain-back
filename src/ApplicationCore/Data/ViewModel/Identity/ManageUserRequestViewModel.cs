namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using Farsica.Framework.DataAnnotation;

    public sealed class ManageUserRequestViewModel
    {
        [Display]
        [Required]
        [StringLength(256)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public required string Username { get; set; }

        [Display]
        public string? Domain { get; set; }

        [Display]
        public string? Password { get; set; }

        [Display]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }

        [Display]
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Display]
        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Display]
        public IEnumerable<string>? AllowedIpAddressesList { get; set; }
    }
}
