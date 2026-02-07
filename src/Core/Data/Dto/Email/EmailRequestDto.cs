namespace GamaEdtech.Data.Dto.Email
{
    using System.Collections.Generic;

    public sealed class EmailRequestDto
    {
        public required string SenderName { get; set; }
        public required string Body { get; set; }
        public required string Subject { get; set; }
        public required IEnumerable<string> Receivers { get; set; }
    }
}
