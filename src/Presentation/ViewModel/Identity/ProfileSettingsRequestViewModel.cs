namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public class ProfileSettingsRequestViewModel
    {
        [Display]
        [TimeZoneId]
        public string? TimeZoneId { get; set; }
    }
}
