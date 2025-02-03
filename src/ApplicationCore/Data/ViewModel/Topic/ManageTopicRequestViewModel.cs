namespace GamaEdtech.Backend.Data.ViewModel.Topic
{
    using Farsica.Framework.DataAnnotation;

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
