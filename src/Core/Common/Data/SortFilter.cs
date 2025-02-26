namespace GamaEdtech.Common.Data
{
    using GamaEdtech.Common.DataAnnotation;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class SortFilter
    {
        public SortType SortType { get; set; }

        [Required]
        public string? Column { get; set; }
    }
}
