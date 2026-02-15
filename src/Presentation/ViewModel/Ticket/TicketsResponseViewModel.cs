namespace GamaEdtech.Presentation.ViewModel.Ticket
{
    using System.Collections.Generic;

    public sealed class TicketsResponseViewModel
    {
        public long Id { get; set; }
        public string? Sender { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public bool IsReadByAdmin { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public IEnumerable<string?>? Receivers { get; set; }
    }
}
