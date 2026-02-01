namespace GamaEdtech.Presentation.ViewModel.Ticket
{
    public sealed class TicketResponseViewModel
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
