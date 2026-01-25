namespace GamaEdtech.Data.Dto.Payment
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class PaymentInfoResponseDto
    {
        public string? Memo { get; set; }
        public string? SourceWallet { get; set; }
        public string? DestinationWallet { get; set; }
        public decimal? Amount { get; set; }
        public string? Mint { get; set; }
        public Currency? Currency { get; set; }
    }
}
