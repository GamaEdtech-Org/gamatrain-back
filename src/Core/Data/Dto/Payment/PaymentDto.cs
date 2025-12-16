namespace GamaEdtech.Data.Dto.Payment
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class PaymentDto
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? VerifyDate { get; set; }
        public string? SourceWallet { get; set; }
        public string? Comment { get; set; }
        public string? TransactionId { get; set; }
    }
}
