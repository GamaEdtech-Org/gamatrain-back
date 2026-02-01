namespace GamaEdtech.Data.Dto.Ticket
{
    public sealed class TicketsDto
    {
        public long Id { get; set; }
        public string? Sender { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public bool IsRead { get; set; }
    }
}
