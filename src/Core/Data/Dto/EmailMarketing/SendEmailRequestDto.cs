namespace GamaEdtech.Data.Dto.EmailMarketing
{
    using System.Collections.Generic;

    public sealed class SendEmailRequestDto
    {
        public required int CreationUserId { get; set; }
        public required DateTimeOffset CreationDate { get; set; }
        public required string Body { get; set; }
        public required string Subject { get; set; }
        public required IEnumerable<int> Users { get; set; }
        public IEnumerable<string>? EmailAddresses { get; set; }
    }
}
