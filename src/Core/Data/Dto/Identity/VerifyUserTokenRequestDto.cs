namespace GamaEdtech.Data.Dto.Identity
{
    public class VerifyUserTokenRequestDto
    {
        public required string TokenProvider { get; set; }

        public required string Purpose { get; set; }

        public required string Token { get; set; }
    }
}
