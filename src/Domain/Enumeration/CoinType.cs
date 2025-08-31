namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class CoinType : Enumeration<CoinType, byte>
    {
        [Display]
        public static readonly CoinType Bronze = new(nameof(Bronze), 0, 1);

        [Display]
        public static readonly CoinType Silver = new(nameof(Silver), 1, 3);

        [Display]
        public static readonly CoinType Gold = new(nameof(Gold), 2, 6);

        public int Points { get; }

        public CoinType()
        {
        }

        public CoinType(string name, byte value, int points) : base(name, value) => Points = points;
    }
}
