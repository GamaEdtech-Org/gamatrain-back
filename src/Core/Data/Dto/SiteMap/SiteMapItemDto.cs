namespace GamaEdtech.Data.Dto.SiteMap
{
    public sealed class SiteMapItemDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset LastModifyDate { get; set; }
    }
}
