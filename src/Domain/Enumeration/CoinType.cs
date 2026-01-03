namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class CoinType : Enumeration<CoinType, byte>
    {
        [Display]
        public static readonly CoinType Bronze = new(nameof(Bronze), 0, "EasterEggBronzePoints");

        [Display]
        public static readonly CoinType Silver = new(nameof(Silver), 1, "EasterEggSilverPoints");

        [Display]
        public static readonly CoinType Gold = new(nameof(Gold), 2, "EasterEggGoldPoints");

        public string ApplicationSettingsName { get; }

        public CoinType()
        {
        }

        public CoinType(string name, byte value, string applicationSettingsName)
            : base(name, value) => ApplicationSettingsName = applicationSettingsName;
    }
}
