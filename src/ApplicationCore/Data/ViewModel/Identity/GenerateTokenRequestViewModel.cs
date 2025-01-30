namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class GenerateTokenRequestViewModel
    {
        [Display]
        [Required]
        public string? Username { get; set; }

        [Display]
        [Required]
        public string? Password { get; set; }

        [Display]
        public string? Domain { get; set; }
    }
}
