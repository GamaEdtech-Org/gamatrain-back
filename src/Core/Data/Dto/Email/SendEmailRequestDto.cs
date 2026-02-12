namespace GamaEdtech.Data.Dto.Email
{
    using System.Collections.Generic;

    public sealed class SendEmailRequestDto
    {
        public required string Body { get; set; }
        public string? From { get; set; }
        public required string Subject { get; set; }
        public required IEnumerable<string> EmailAddresses { get; set; }
    }
}
