namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class SpendPointsRequestViewModel
    {
        [Required]
        public long? Points { get; set; }
    }
}
