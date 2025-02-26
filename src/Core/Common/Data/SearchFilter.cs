namespace GamaEdtech.Common.Data
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class SearchFilter
    {
        [Required]
        public string? Phrase { get; set; }

        [Required]
        public string? Column { get; set; }
    }
}
