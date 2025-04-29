namespace GamaEdtech.Presentation.ViewModel.Transaction
{
    public sealed class TransactionsResponseViewModel
    {
        public long Id { get; set; }
        public int Points { get; set; }
        public string? Description { get; set; }
        public int CurrentBalance { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool IsDebit { get; set; }
    }
}
