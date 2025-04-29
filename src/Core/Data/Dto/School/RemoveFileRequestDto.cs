namespace GamaEdtech.Data.Dto.School
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class RemoveFileRequestDto
    {
        public required string FileId { get; set; }
        public required ContainerType ContainerType { get; set; }
    }
}
