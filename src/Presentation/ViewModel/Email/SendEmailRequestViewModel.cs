namespace GamaEdtech.Presentation.ViewModel.Email
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    public sealed class SendEmailRequestViewModel
    {
        [Display]
        [Required]
        [JsonConverter(typeof(EnumerationConverter<Sender, byte>))]
        public Sender? Sender { get; set; }

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
