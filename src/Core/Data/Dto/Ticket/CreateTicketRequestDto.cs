namespace GamaEdtech.Data.Dto.Ticket
{
    using Microsoft.AspNetCore.Http;

    public sealed class CreateTicketRequestDto
    {
        public string? Sender { get; set; }
        public string? Email { get; set; }
        public required string? Subject { get; set; }
        public required string? Body { get; set; }
        public int? CreationUserId { get; set; }
        public long? ParentId { get; set; }
        public IFormFile? File { get; set; }
    }
}
