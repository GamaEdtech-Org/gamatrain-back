namespace GamaEdtech.Data.Dto.Payment
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class VerifyPaymentRequestDto
    {
        public required int UserId { get; set; }
        public required long Id { get; set; }
        public required Currency Currency { get; set; }
        public required string TransactionId { get; set; }
    }
}
