namespace GamaEdtech.Backend.Data.ViewModel.Topic
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    public sealed class TopicsRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public int? SubjectId { get; set; }
    }
}
