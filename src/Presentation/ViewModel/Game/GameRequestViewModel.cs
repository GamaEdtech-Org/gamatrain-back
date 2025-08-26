namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class GameRequestViewModel
    {
        [Required]
        [Range(1, 1000)]
        public int? Points { get; set; }
    }
}
