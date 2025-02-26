namespace GamaEdtech.Common.Identity
{
    public class VerifyTokenRequest
    {
        public string? TokenProvider { get; set; }

        public string? Purpose { get; set; }

        public string? UserId { get; set; }

        public string? Token { get; set; }
    }
}
