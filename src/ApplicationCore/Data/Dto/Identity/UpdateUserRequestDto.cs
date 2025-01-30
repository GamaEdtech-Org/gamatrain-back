namespace GamaEdtech.Backend.Data.Dto.Identity
{
    public class UpdateUserRequestDto
    {
        public required int Id { get; set; }

        public required string Username { get; set; }

        public string? Password { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
