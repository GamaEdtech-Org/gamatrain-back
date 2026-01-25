namespace GamaEdtech.Presentation.ViewModel.Payment
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Domain.Enumeration;

    public sealed class PaymentsListResponseViewModel
    {
        public long Id { get; set; }

        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public decimal Amount { get; set; }

        [JsonConverter(typeof(EnumerationConverter<Currency, byte>))]
        public Currency Currency { get; set; }

        [JsonConverter(typeof(EnumerationConverter<PaymentStatus, byte>))]
        public PaymentStatus Status { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset? VerifyDate { get; set; }

        public string? SourceWallet { get; set; }

        public string? Comment { get; set; }

        public string? TransactionId { get; set; }
    }
}
