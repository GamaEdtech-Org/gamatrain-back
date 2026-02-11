namespace GamaEdtech.Presentation.ViewModel.Email
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class SendEmailRequestViewModel
    {
        [Display]
        [Required]
        public string? SenderName { get; set; }

        [Display]
        [Required]
        public string? Body { get; set; }

        [Display]
        [Required]
        public string? Subject { get; set; }

        [Display]
        [Required]
        public IEnumerable<int>? Users { get; set; }

        [Display]
        [EmailAddress]
        public IEnumerable<string>? EmailAddresses { get; set; }
    }
}
