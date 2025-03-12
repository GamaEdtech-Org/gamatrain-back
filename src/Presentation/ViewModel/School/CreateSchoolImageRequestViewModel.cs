namespace GamaEdtech.Presentation.ViewModel.School
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;

    public sealed class CreateSchoolImageRequestViewModel
    {
        [Display]
        [Required]
        public IFormFile? File { get; set; }

        [Display]
        [Required]
        [JsonConverter(typeof(EnumerationConverter<FileType, byte>))]
        public FileType? FileType { get; set; }
    }
}
