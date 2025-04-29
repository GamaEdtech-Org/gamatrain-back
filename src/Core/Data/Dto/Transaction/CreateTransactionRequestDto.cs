namespace GamaEdtech.Data.Dto.Transaction
{
    public sealed class CreateTransactionRequestDto
    {
        public int UserId { get; set; }
        public long? IdentifierId { get; set; }
        public int Points { get; set; }
        public string? Description { get; set; }
    }
}
