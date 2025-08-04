namespace GamaEdtech.Data.Dto.Identity
{

    public class ManageProfileSettingsRequestDto
    {
        public required int UserId { get; set; }
        public int? CityId { get; set; }
        public long? SchoolId { get; set; }
    }
}
