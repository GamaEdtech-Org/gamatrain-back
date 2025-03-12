namespace GamaEdtech.Presentation.ViewModel.Tag
{
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Converter;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    public sealed class TagsRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        [JsonConverter(typeof(EnumerationConverter<TagType, byte>))]
        public TagType? TagType { get; set; }
    }
}
