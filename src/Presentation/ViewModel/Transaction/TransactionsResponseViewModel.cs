namespace GamaEdtech.Presentation.ViewModel.Transaction
{
    public sealed class TransactionsResponseViewModel
    {
        public long Id { get; set; }
        public long Points { get; set; }
        public string? Description { get; set; }
        public long CurrentBalance { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool IsDebit { get; set; }
    }
}
