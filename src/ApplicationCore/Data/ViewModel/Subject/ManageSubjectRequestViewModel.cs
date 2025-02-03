namespace GamaEdtech.Backend.Data.ViewModel.Subject
{
    using Farsica.Framework.DataAnnotation;

    public sealed class ManageSubjectRequestViewModel
    {
        [Display]
        [Required]
        public string? Title { get; set; }

        [Display]
        [Required]
        public int Order { get; set; }
    }
}
