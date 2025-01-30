namespace GamaEdtech.Backend.Data.Dto.Identity
{
    public sealed class GenerateUserTokenResponseDto
    {
        public string? Token { get; set; }

        public DateTimeOffset? ExpirationTime { get; set; }
    }
}
