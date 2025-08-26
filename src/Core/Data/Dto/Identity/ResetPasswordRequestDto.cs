namespace GamaEdtech.Data.Dto.Identity
{
    public sealed class ResetPasswordRequestDto
    {
        public required int UserId { get; set; }

        public required string NewPassword { get; set; }
    }
}
