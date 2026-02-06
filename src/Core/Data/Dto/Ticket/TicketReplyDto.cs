namespace GamaEdtech.Data.Dto.Ticket
{
    using System;

    public sealed class TicketReplyDto
    {
        public long Id { get; set; }
        public string? CreationUser { get; set; }
        public string? Body { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Uri? FileUri { get; set; }
    }
}
