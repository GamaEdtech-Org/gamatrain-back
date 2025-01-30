namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using Farsica.Framework.DataAnnotation;

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
