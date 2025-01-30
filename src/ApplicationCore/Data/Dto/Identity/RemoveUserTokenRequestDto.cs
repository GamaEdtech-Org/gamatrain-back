namespace GamaEdtech.Backend.Data.Dto.Identity
{
    public class RemoveUserTokenRequestDto
    {
        public required int UserId { get; set; }

        public required string TokenProvider { get; set; }

        public required string Purpose { get; set; }
    }
}
