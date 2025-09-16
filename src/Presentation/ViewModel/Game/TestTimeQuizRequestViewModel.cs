namespace GamaEdtech.Presentation.ViewModel.Game
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class TestTimeQuizRequestViewModel
    {
        [Display]
        [Required]
        public long? TestId { get; set; }

        [Display]
        [Required]
        public int? SubmissionId { get; set; }
    }
}
