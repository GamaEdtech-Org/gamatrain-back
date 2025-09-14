namespace GamaEdtech.Presentation.ViewModel.Payment
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    public sealed class CreatePaymentRequestViewModel
    {
        [Display]
        [Required]
        public decimal? Amount { get; set; }

        [Display]
        [Required]
        [JsonConverter(typeof(EnumerationConverter<Currency, byte>))]
        public Currency? Currency { get; set; }
    }
}
