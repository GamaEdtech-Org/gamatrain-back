namespace GamaEdtech.Presentation.ViewModel.Ticket
{
    using System;

    public sealed class TicketResponseViewModel
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Uri? FileUri { get; set; }
    }
}
