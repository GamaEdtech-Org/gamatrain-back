namespace GamaEdtech.Data.Dto.File
{
    using Microsoft.AspNetCore.Http;

    public sealed class CreateFileRequestDto
    {
        public IFormFile? File { get; set; }
    }
}
