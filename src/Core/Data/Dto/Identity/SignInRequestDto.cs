namespace GamaEdtech.Data.Dto.Identity
{
    public sealed class SignInRequestDto
    {
        public required ApplicationUserDto User { get; set; }

        public bool RememberMe { get; set; }
    }
}
