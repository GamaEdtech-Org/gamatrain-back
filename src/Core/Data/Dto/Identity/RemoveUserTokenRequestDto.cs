namespace GamaEdtech.Data.Dto.Identity
{
    public sealed class RemoveUserTokenRequestDto
    {
        public required int UserId { get; set; }

        public required string TokenProvider { get; set; }

        public required string Purpose { get; set; }
    }
}
