namespace GamaEdtech.Data.Dto.Contribution
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class ManageContributionRequestDto
    {
        public long? Id { get; set; }
        public required CategoryType CategoryType { get; set; }
        public required Status Status { get; set; }
        public string? Comment { get; set; }
        public string? Data { get; set; }
        public long? IdentifierId { get; set; }
    }
}
