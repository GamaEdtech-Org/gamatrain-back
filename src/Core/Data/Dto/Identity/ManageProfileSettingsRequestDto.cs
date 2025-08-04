namespace GamaEdtech.Data.Dto.Identity
{

    public class ManageProfileSettingsRequestDto
    {
        public required int UserId { get; set; }
        public int? CityId { get; set; }
        public int? SchoolId { get; set; }
    }
}
