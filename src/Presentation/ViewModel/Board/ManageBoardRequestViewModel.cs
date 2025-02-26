namespace GamaEdtech.Presentation.ViewModel.Board
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ManageBoardRequestViewModel
    {
        [Display]
        [Required]
        public string? Title { get; set; }

        [Display]
        public string? Description { get; set; }

        [Display]
        public string? Icon { get; set; }
    }
}
