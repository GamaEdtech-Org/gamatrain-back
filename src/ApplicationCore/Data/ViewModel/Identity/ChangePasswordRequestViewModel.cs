namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using Farsica.Framework.DataAnnotation;

    public sealed class ChangePasswordRequestViewModel
    {
        [Display]
        [Required]
        public required string CurrentPassword { get; set; }

        [Display]
        [Required]
        public required string NewPassword { get; set; }

        [Display]
        [Required]
        [Compare(nameof(NewPassword))]
        public required string ConfirmNewPassword { get; set; }
    }
}
