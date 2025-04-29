namespace GamaEdtech.Data.Dto.Transaction
{
    public sealed class TransactionDto
    {
        public long Id { get; set; }
        public int Points { get; set; }
        public string? Description { get; set; }
        public int CurrentBalance { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool IsDebit { get; set; }
        public int UserId { get; set; }
    }
}
