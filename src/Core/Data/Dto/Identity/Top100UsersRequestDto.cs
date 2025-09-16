namespace GamaEdtech.Data.Dto.Identity
{
    public sealed class Top100UsersRequestDto
    {
        public int? Board { get; set; }
        public int? Grade { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public long? SchoolId { get; set; }
        public DateTimeOffset? RegistrationDateStart { get; set; }
        public DateTimeOffset? RegistrationDateEnd { get; set; }
    }
}
