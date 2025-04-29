namespace GamaEdtech.Presentation.ViewModel.Transaction
{
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Domain.Enumeration;

    public sealed class TransactionStatisticsRequestViewModel
    {
        [Display]
        public DateTime? StartDate { get; set; }

        [Display]
        public DateTime? EndDate { get; set; }

        [Display]
        [Required]
        public Period Period { get; set; }
    }
}
