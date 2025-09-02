namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class PointRequestViewModel
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
