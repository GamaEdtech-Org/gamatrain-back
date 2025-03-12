namespace GamaEdtech.Data.Dto.File
{
    using Microsoft.AspNetCore.Http;

    public sealed class CreateFileRequestDto
    {
        public required IFormFile File { get; set; }
    }
}
