namespace GamaEdtech.Data.Dto.Contribution
{
    using System;

    using GamaEdtech.Domain.Enumeration;

    public sealed class ContributionDto
    {
        public long Id { get; set; }
        public string? Comment { get; set; }
        public string? Data { get; set; }
        public long? IdentifierId { get; set; }
        public CategoryType? CategoryType { get; set; }
        public int CreationUserId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
