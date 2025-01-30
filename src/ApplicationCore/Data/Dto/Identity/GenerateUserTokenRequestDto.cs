namespace GamaEdtech.Backend.Data.Dto.Identity
{
    public class GenerateUserTokenRequestDto
    {
        public required int UserId { get; set; }

        public required string TokenProvider { get; set; }

        public required string Purpose { get; set; }
    }
}
