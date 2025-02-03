namespace GamaEdtech.Backend.Data.ViewModel.Grade
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    public sealed class GradesRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };

        [Display]
        public int? BoardId { get; set; }
    }
}
