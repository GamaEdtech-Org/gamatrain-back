namespace GamaEdtech.Data.Dto.Identity
{
    using GamaEdtech.Domain.Enumeration;

    public class AuthenticationRequestDto
    {
        public required string Username { get; set; }

        public string? Password { get; set; }

        public required AuthenticationProvider AuthenticationProvider { get; set; }
    }
}
