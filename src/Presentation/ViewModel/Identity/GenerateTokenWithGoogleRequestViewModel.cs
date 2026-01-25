namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class GenerateTokenWithGoogleRequestViewModel
    {
        [Display]
        [Required]
        public string? Code { get; set; }
    }
}
