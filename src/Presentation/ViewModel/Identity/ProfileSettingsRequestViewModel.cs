namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;

    public sealed class ProfileSettingsRequestViewModel
    {
        [Display]
        public int? CityId { get; set; }

        [Display]
        public long? SchoolId { get; set; }

        [Display]
        public string? FirstName { get; set; }

        [Display]
        public string? LastName { get; set; }

        [Display]
        [JsonConverter(typeof(EnumerationConverter<GenderType, byte>))]
        public GenderType? Gender { get; set; }

        [Display]
        public int? Section { get; set; }

        [Display]
        public int? Grade { get; set; }

        [Display]
        [FileSize(300 * 1024)] // 300KB
        [FileExtensions(Constants.ValidImageExtensions)]
        public IFormFile? Avatar { get; set; }

        [Display]
        public string? UserName { get; set; }
    }
}
