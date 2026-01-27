namespace GamaEdtech.Presentation.ViewModel.ApplicationSettings
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    public sealed class ManageSiteMapRequestViewModel
    {
        [Display]
        [JsonConverter(typeof(EnumerationConverter<ChangeFrequency, byte>))]
        public ChangeFrequency? ChangeFrequency { get; set; }

        [Display]
        public double? Priority { get; set; }
    }
}
