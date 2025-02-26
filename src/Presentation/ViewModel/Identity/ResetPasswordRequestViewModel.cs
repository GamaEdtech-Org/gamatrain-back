namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ResetPasswordRequestViewModel
    {
        [Display]
        [Required]
        public required string NewPassword { get; set; }

        [Display]
        [Required]
        [Compare(nameof(NewPassword))]
        public required string ConfirmNewPassword { get; set; }
    }
}
