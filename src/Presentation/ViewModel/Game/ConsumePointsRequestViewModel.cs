namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ConsumePointsRequestViewModel
    {
        [Required]
        public int? Points { get; set; }
    }
}
