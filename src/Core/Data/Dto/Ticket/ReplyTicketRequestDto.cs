namespace GamaEdtech.Data.Dto.Ticket
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public sealed class ReplyTicketRequestDto
    {
        public required long TicketId { get; set; }
        public required string Body { get; set; }
        public required bool ReplyByAdmin { get; set; }
        public string? From { get; set; }
        public int? CreationUserId { get; set; }
        public IFormFile? File { get; set; }
        public ICollection<string?>? Receivers { get; set; }
    }
}
