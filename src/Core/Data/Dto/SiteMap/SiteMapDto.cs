namespace GamaEdtech.Data.Dto.SiteMap
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class SiteMapDto
    {
        public long Id { get; set; }
        public long IdentifierId { get; set; }
        public ItemType ItemType { get; set; }
        public ChangeFrequency? ChangeFrequency { get; set; }
        public double? Priority { get; set; }
    }
}
