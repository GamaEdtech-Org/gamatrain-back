namespace GamaEdtech.Presentation.ViewModel.Ticket
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class CreateTicketRequestViewModel
    {
        [Display]
        [Required]
        public string? Captcha { get; set; }

        [Display]
        [Required]
        public string? FullName { get; set; }

        [Display]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Display]
        [Required]
        public string? Subject { get; set; }

        [Display]
        [Required]
        public string? Body { get; set; }
    }
}
