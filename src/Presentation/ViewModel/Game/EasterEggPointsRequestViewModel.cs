namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class EasterEggPointsRequestViewModel
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
