namespace GamaEdtech.Presentation.ViewModel.Topic
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ManageTopicRequestViewModel
    {
        [Display]
        [Required]
        public string? Title { get; set; }

        [Display]
        [Required]
        public int Order { get; set; }
    }
}
