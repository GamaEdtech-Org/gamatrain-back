namespace GamaEdtech.Data.Dto.School
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class UploadFileRequestDto
    {
        public required byte[] File { get; set; }
        public required string FileExtension { get; set; }
        public required ContainerType ContainerType { get; set; }
    }
}
