namespace GamaEdtech.Presentation.ViewModel.School
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;

    public class ManageNewSchoolContributionRequestViewModel
    {
        [Display]
        [Required]
        public virtual string? Name { get; set; }

        [Display]
        [Required]
        public virtual double? Latitude { get; set; }

        [Display]
        [Required]
        public virtual double? Longitude { get; set; }

        [Display]
        [Required]
        public virtual int? StateId { get; set; }

        [Display]
        [Required]
        public virtual int? CityId { get; set; }

        [Display]
        [Required]
        public virtual int? CountryId { get; set; }

        [Display]
        public string? LocalName { get; set; }

        [Display]
        [JsonConverter(typeof(EnumerationConverter<SchoolType, byte>))]
        public SchoolType? SchoolType { get; set; }

        [Display]
        public string? ZipCode { get; set; }

        [Display]
        public string? Address { get; set; }

        [Display]
        public string? WebSite { get; set; }

        [Display]
        public string? LocalAddress { get; set; }

        [Display]
        public string? Email { get; set; }

        [Display]
        public string? FaxNumber { get; set; }

        [Display]
        public string? PhoneNumber { get; set; }

        [Display]
        public string? Quarter { get; set; }

        [Display]
        public IEnumerable<long>? Tags { get; set; }

        [Display]
        public decimal? Tuition { get; set; }

        [Display]
        public string? Description { get; set; }

        [Display]
        public ManageSchoolCommentRequestViewModel? Comment { get; set; }

        [Display]
        public IFormFile? File { get; set; }
    }
}
