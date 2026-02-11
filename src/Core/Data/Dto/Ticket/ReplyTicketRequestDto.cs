namespace GamaEdtech.Data.Dto.Ticket
{
    using Microsoft.AspNetCore.Http;

    public sealed class ReplyTicketRequestDto
    {
        public required long TicketId { get; set; }
        public required string Body { get; set; }
        public required bool ReplyByAdmin { get; set; }
        public IEnumerable<string?>? Operators { get; set; }
        public int? CreationUserId { get; set; }
        public IFormFile? File { get; set; }
    }
}
