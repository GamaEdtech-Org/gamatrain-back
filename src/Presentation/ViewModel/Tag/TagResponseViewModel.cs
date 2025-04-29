namespace GamaEdtech.Presentation.ViewModel.Tag
{
    using GamaEdtech.Common.Converter;
    using System.Text.Json.Serialization;

    using GamaEdtech.Domain.Enumeration;

    public sealed class TagResponseViewModel
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Icon { get; set; }

        [JsonConverter(typeof(EnumerationConverter<TagType, byte>))]
        public TagType? TagType { get; set; }
    }
}
