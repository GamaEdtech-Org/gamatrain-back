namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

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
