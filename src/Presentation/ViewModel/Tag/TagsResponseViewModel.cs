namespace GamaEdtech.Presentation.ViewModel.Tag
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Domain.Enumeration;

    public sealed class TagsResponseViewModel
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Icon { get; set; }

        [JsonConverter(typeof(EnumerationConverter<TagType, byte>))]
        public TagType? TagType { get; set; }
    }
}
