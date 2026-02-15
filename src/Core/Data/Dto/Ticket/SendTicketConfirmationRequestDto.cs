namespace GamaEdtech.Data.Dto.Ticket
{
    public sealed class SendTicketConfirmationRequestDto
    {
        public required long TicketId { get; set; }
        public required string? ReceiverName { get; set; }
        public required string? ReceiverEmail { get; set; }
        public required string? Subject { get; set; }
        public required string? Body { get; set; }
        public string? From { get; set; }
    }
}
