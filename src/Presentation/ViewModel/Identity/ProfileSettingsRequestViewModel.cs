namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ProfileSettingsRequestViewModel
    {
        [Display]
        public int? CityId { get; set; }

        [Display]
        public long? SchoolId { get; set; }
    }
}
