namespace GamaEdtech.Data.Dto.Payment
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class CreatePaymentRequestDto
    {
        public required int UserId { get; set; }
        public required decimal Amount { get; set; }
        public required Currency Currency { get; set; }
    }
}
