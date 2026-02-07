namespace GamaEdtech.Data.Dto.Ticket
{
    using Microsoft.AspNetCore.Http;

    public sealed class CreateTicketRequestDto
    {
        public required string? FullName { get; set; }
        public required string? Email { get; set; }
        public required string? Subject { get; set; }
        public required string? Body { get; set; }
        public int? CreationUserId { get; set; }
        public IFormFile? File { get; set; }
    }
}
