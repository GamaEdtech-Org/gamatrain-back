namespace GamaEdtech.Data.Dto.Transaction
{
    public sealed class CreateTransactionRequestDto
    {
        public int UserId { get; set; }
        public long? IdentifierId { get; set; }
        public long Points { get; set; }
        public string? Description { get; set; }
    }
}
