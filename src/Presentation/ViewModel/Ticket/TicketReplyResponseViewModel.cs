namespace GamaEdtech.Presentation.ViewModel.Ticket
{
    using System;

    public sealed class TicketReplyResponseViewModel
    {
        public long Id { get; set; }
        public string? Body { get; set; }
        public string? CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Uri? FileUri { get; set; }
    }
}
