namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class GenerateTokenWithOldRequestViewModel
    {
        [Display]
        [Required]
        public string? Token { get; set; }
    }
}
