namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using Farsica.Framework.DataAnnotation;

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

        [Display]
        public string? Domain { get; set; }
    }
}
