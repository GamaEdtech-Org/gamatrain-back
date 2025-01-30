namespace GamaEdtech.Backend.Data.Dto.Identity
{
    public sealed class VerifyUserTokenResponseDto
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? TimeZoneId { get; set; }
    }
}
