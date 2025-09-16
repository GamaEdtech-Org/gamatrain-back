namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class TestTimeQuizRequestViewModel
    {
        [Display]
        [Required]
        public long? Id { get; set; }

        [Display]
        [Required]
        public int? Answer { get; set; }
    }
}
