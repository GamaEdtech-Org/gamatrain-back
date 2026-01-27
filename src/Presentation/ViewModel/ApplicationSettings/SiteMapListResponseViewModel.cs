namespace GamaEdtech.Presentation.ViewModel.ApplicationSettings
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Domain.Enumeration;

    public sealed class SiteMapListResponseViewModel
    {
        public long Id { get; set; }

        public long IdentifierId { get; set; }

        public string? Title { get; set; }

        [JsonConverter(typeof(EnumerationConverter<ChangeFrequency, byte>))]
        public ChangeFrequency? ChangeFrequency { get; set; }

        public double? Priority { get; set; }
    }
}
