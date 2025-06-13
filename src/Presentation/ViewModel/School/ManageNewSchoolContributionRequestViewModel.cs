namespace GamaEdtech.Presentation.ViewModel.School
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

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

        public string? LocalName { get; set; }

        [JsonConverter(typeof(EnumerationConverter<SchoolType, byte>))]
        public SchoolType? SchoolType { get; set; }

        public string? ZipCode { get; set; }

        public string? Address { get; set; }

        public string? WebSite { get; set; }

        public string? LocalAddress { get; set; }

        public string? Email { get; set; }

        public string? FaxNumber { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Quarter { get; set; }

        public IEnumerable<long>? Tags { get; set; }
    }
}
