namespace GamaEdtech.Backend.Data.ViewModel.Identity
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    public sealed class ConsumersRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };
    }
}
