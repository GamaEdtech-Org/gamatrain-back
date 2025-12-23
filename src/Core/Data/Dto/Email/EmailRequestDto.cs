namespace GamaEdtech.Data.Dto.Email
{
    using System.Collections.Generic;

    using GamaEdtech.Domain.Enumeration;

    public sealed class EmailRequestDto
    {
        public required Sender Sender { get; set; }
        public required string Body { get; set; }
        public required string Subject { get; set; }
        public required IEnumerable<string> Receivers { get; set; }
    }
}
