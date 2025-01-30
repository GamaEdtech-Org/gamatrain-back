namespace GamaEdtech.Backend.Data.Dto.Identity
{
    public class AuthenticationRequestDto
    {
        public required string Username { get; set; }

        public required string Password { get; set; }

        public string? Domain { get; set; }
    }
}
