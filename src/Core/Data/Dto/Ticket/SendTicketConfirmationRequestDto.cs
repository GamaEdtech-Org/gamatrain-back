namespace GamaEdtech.Data.Dto.Ticket
{
    public sealed class SendTicketConfirmationRequestDto
    {
        public required long TicketId { get; set; }
        public required string? FullName { get; set; }
        public required string? Email { get; set; }
        public required string? Subject { get; set; }
        public required string? Body { get; set; }
    }
}
