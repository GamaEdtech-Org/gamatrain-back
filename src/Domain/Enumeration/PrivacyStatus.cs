namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class PrivacyStatus : Enumeration<Status, byte>
    {
        [Display]
        public static readonly Status Public = new(nameof(Public), 0);

        [Display]
        public static readonly Status Private = new(nameof(Private), 1);

        [Display]
        public static readonly Status Unlisted = new(nameof(Unlisted), 2);

        public PrivacyStatus()
        {
        }

        public PrivacyStatus(string name, byte value) : base(name, value)
        {
        }
    }
}
