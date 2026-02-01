namespace GamaEdtech.Data.Dto.SiteMap
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class ManageSiteMapRequestDto
    {
        public long? Id { get; set; }
        public required long IdentifierId { get; set; }
        public required ItemType ItemType { get; set; }
        public required ChangeFrequency? ChangeFrequency { get; set; }
        public required double? Priority { get; set; }
    }
}
