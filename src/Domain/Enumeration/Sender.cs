namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class Sender : Enumeration<Sender, byte>
    {
        [Display]
        public static readonly Sender Info = new(nameof(Info), 0, "Info");

        [Display]
        public static readonly Sender Marketing = new(nameof(Marketing), 1, "Marketing");

        [Display]
        public static readonly Sender Support = new(nameof(Support), 2, "Support");

        public string ApplicationSettingsName { get; }

        public Sender()
        {
        }

        public Sender(string name, byte value, string applicationSettingsName)
            : base(name, value) => ApplicationSettingsName = applicationSettingsName;
    }
}
