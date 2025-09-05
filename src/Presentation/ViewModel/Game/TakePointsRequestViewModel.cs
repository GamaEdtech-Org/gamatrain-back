namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class TakePointsRequestViewModel
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
