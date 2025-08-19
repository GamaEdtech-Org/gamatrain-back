namespace GamaEdtech.Presentation.ViewModel.School
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Domain.Enumeration;

    public sealed class SchoolImageContributionReviewViewModel
    {
        public long Id { get; set; }

        public string? SchoolName { get; set; }

        public long SchoolId { get; set; }

        public Uri? FileUri { get; set; }


        [JsonConverter(typeof(EnumerationConverter<FileType, byte>))]
        public FileType? FileType { get; set; }

        public bool IsDefault { get; set; }

        public long? TagId { get; set; }

        public string? TagName { get; set; }
    }
}
