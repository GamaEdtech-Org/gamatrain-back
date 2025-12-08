namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class EmailProviderType : Enumeration<EmailProviderType, byte>
    {
        [Display]
        public static readonly EmailProviderType Resend = new(nameof(Resend), 0);

        public EmailProviderType()
        {
        }

        public EmailProviderType(string name, byte value) : base(name, value)
        {
        }
    }
}
