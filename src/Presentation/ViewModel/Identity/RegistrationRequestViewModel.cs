namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class RegistrationRequestViewModel
    {
        [Display]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Display]
        [Required]
        public string? Password { get; set; }

        [Display]
        [Required]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
