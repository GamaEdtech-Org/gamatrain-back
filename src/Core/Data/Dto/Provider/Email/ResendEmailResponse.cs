namespace GamaEdtech.Data.Dto.Provider.Email
{
    using System.Text.Json.Serialization;

    public sealed class ResendEmailResponse
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreationDate { get; set; }

        [JsonPropertyName("data")]
        public DataDto Data { get; set; }

        public sealed class DataDto
        {
            [JsonPropertyName("email_id")]
            public Guid EmailId { get; set; }

            [JsonPropertyName("created_at")]
            public DateTimeOffset CreationDate { get; set; }

            [JsonPropertyName("from")]
            public string? From { get; set; }

            [JsonPropertyName("to")]
            public IEnumerable<string?> To { get; set; }

            [JsonPropertyName("bcc")]
            public IEnumerable<string?> Bcc { get; set; }

            [JsonPropertyName("cc")]
            public IEnumerable<string?> Cc { get; set; }

            [JsonPropertyName("message_id")]
            public string? MessageId { get; set; }

            [JsonPropertyName("subject")]
            public string? Subject { get; set; }

            [JsonPropertyName("attachments")]
            public IEnumerable<AttachmentDto>? Attachments { get; set; }

            public sealed class AttachmentDto
            {
                [JsonPropertyName("id")]
                public Guid Id { get; set; }

                [JsonPropertyName("filename")]
                public string? Filename { get; set; }

                [JsonPropertyName("content_type")]
                public string? ContentType { get; set; }

                [JsonPropertyName("content_disposition")]
                public string? ContentDisposition { get; set; }

                [JsonPropertyName("content_id")]
                public string? ContentId { get; set; }
            }
        }
    }
}
