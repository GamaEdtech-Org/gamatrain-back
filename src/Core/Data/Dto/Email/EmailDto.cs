namespace GamaEdtech.Data.Dto.Email
{
    using System.Collections.Generic;

    public sealed class EmailDto
    {
        public string? From { get; set; }
        public IEnumerable<string?>? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public IReadOnlyList<AttachmentDto>? Attachments { get; set; }

        public sealed class AttachmentDto
        {
            public byte[] File { get; set; }
            public string? Filename { get; set; }
            public string? ContentType { get; set; }
        }
    }
}
