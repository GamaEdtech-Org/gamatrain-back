namespace GamaEdtech.Backend.Data.ViewModel.Board
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    public sealed class BoardsRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };
    }
}
