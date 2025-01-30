namespace GamaEdtech.Backend.Data.ViewModel.Location
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    public sealed class LocationsRequestViewModel
    {
        [Display]
        public PagingDto? PagingDto { get; set; } = new() { PageFilter = new(), };
    }
}
