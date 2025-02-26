namespace GamaEdtech.Data.Dto.Identity
{
    public class RegistrationRequestDto
    {
        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
