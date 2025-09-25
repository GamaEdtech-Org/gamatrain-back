namespace GamaEdtech.Presentation.ViewModel.Transaction
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class TransactionsListRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public bool? IsDebit { get; set; }

        [Display]
        public int? UserId { get; set; }

        [Display]
        public long? IdentifierId { get; set; }

        [Display]
        public DateTimeOffset? StartDate { get; set; }

        [Display]
        public DateTimeOffset? EndDate { get; set; }
    }
}
