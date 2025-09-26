namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ExamPointsRequestViewModel
    {
        [Required]
        public long? Id { get; set; }
    }
}
