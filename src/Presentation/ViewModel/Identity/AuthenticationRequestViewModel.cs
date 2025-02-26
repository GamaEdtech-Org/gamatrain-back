namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class AuthenticationRequestViewModel
    {
        [Display]
        [Required]
        public string? Username { get; set; }

        [Display]
        [Required]
        public string? Password { get; set; }

        [Display]
        public bool RememberMe { get; set; }
    }
}
