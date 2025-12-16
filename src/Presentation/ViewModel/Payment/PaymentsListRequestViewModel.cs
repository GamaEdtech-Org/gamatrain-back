namespace GamaEdtech.Presentation.ViewModel.Payment
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class PaymentsListRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public int? UserId { get; set; }

        public long? IdentifierId { get; set; }

        [Display]
        public DateTimeOffset? StartDate { get; set; }

        [Display]
        public DateTimeOffset? EndDate { get; set; }
    }
}
