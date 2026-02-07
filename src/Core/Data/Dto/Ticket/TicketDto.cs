namespace GamaEdtech.Data.Dto.Ticket
{
    using System;

    public sealed class TicketDto
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? CreationUser { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Uri? FileUri { get; set; }
    }
}
