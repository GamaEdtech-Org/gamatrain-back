namespace GamaEdtech.Presentation.ViewModel.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class Top100UsersRequestViewModel
    {
        [Display]
        public int? Board { get; set; }

        [Display]
        public int? Grade { get; set; }

        [Display]
        public int? CountryId { get; set; }

        [Display]
        public int? StateId { get; set; }

        [Display]
        public int? CityId { get; set; }

        [Display]
        public long? SchoolId { get; set; }

        [Display]
        public DateTimeOffset? RegistrationDateStart { get; set; }

        [Display]
        public DateTimeOffset? RegistrationDateEnd { get; set; }
    }
}
