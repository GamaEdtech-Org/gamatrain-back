namespace GamaEdtech.Common.Data
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class PageFilter
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Skip { get; set; } = 0;

        [Required]
        [Range(1, int.MaxValue)]
        public int Size { get; set; } = 1;

        public bool ReturnTotalRecordsCount { get; set; }
    }
}
