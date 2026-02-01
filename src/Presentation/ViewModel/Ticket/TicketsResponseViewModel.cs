namespace GamaEdtech.Presentation.ViewModel.Ticket
{
    public sealed class TicketsResponseViewModel
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public bool IsRead { get; set; }
    }
}
