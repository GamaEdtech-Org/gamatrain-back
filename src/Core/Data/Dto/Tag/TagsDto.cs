namespace GamaEdtech.Data.Dto.Tag
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class TagsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public TagType? TagType { get; set; }
    }
}
