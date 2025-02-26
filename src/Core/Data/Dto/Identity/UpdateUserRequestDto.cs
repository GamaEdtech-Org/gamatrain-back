namespace GamaEdtech.Data.Dto.Identity
{
    public class UpdateUserRequestDto
    {
        public required int Id { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Avatar { get; set; }
    }
}
