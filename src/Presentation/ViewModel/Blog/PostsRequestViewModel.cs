namespace GamaEdtech.Presentation.ViewModel.Blog
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class PostsRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public int? TagId { get; set; }
    }
}
