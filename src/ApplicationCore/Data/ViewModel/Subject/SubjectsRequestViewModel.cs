namespace GamaEdtech.Backend.Data.ViewModel.Subject
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    public sealed class SubjectsRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public int? GradeId { get; set; }
    }
}
