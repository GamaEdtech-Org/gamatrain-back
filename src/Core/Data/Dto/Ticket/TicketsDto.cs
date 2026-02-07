namespace GamaEdtech.Data.Dto.Ticket
{
    public sealed class TicketsDto
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public bool IsReadByAdmin { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
