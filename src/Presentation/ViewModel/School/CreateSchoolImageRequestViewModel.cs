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
        public long? TagId { get; set; }

        [Display]
        [Required]
        [JsonConverter(typeof(EnumerationConverter<ImageFileType, byte>))]
        public ImageFileType? FileType { get; set; }

        [Display]
        public bool IsDefault { get; set; }
    }
}
