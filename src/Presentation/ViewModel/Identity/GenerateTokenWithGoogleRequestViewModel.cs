namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class GenerateTokenWithGoogleRequestViewModel
    {
        [Display]
        [Required]
        public string? Code { get; set; }
    }
}
