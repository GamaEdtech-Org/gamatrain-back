namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class AuthenticationProvider : Enumeration<AuthenticationProvider, byte>
    {
        [Display]
        public static readonly AuthenticationProvider Local = new(nameof(Local), 0);

        [Display]
        public static readonly AuthenticationProvider Google = new(nameof(Google), 1);

        public AuthenticationProvider()
        {
        }

        public AuthenticationProvider(string name, byte value) : base(name, value)
        {
        }
    }
}
