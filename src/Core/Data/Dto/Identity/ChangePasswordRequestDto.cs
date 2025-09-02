namespace GamaEdtech.Data.Dto.Identity
{
    public sealed class ChangePasswordRequestDto
    {
        public required string CurrentPassword { get; set; }

        public required string NewPassword { get; set; }
    }
}
