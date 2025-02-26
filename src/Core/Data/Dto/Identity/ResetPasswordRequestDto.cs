namespace GamaEdtech.Data.Dto.Identity
{
    public class ResetPasswordRequestDto
    {
        public required int UserId { get; set; }

        public required string NewPassword { get; set; }
    }
}
