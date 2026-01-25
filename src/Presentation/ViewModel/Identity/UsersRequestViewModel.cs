namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class UserListRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public bool? HasReferral { get; set; }
    }
}
