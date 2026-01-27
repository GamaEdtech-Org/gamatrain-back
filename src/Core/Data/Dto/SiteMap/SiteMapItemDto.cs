namespace GamaEdtech.Data.Dto.SiteMap
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class SiteMapItemDto
    {
        public static readonly double DefaultPriority = 1;
        public static readonly ChangeFrequency DefaultChangeFrequency = ChangeFrequency.Monthly;

        public long Id { get; set; }
        public string? Title { get; set; }
        public ItemType ItemType { get; set; }
        public DateTimeOffset LastModifyDate { get; set; }
        public ChangeFrequency ChangeFrequency { get; set; } = DefaultChangeFrequency;
        public double Priority { get; set; } = DefaultPriority;
    }
}
