namespace GamaEdtech.Data.Dto.Tag
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class ManageTagRequestDto
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public string? Icon { get; set; }
        public required TagType TagType { get; set; }
    }
}
