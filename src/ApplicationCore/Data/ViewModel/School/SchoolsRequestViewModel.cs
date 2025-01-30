namespace GamaEdtech.Backend.Data.ViewModel.School
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    public sealed class SchoolsRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };
    }
}
