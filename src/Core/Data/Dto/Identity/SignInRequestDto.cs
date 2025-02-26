namespace GamaEdtech.Data.Dto.Identity
{
    public class SignInRequestDto
    {
        public required ApplicationUserDto User { get; set; }

        public bool RememberMe { get; set; }
    }
}
