namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class CoinType : Enumeration<CoinType, byte>
    {
        [Display]
        public static readonly CoinType Bronze = new(nameof(Bronze), 0, 1000000);   //each 1000000 points means 1 get

        [Display]
        public static readonly CoinType Silver = new(nameof(Silver), 1, 3000000);

        [Display]
        public static readonly CoinType Gold = new(nameof(Gold), 2, 6000000);

        public long Points { get; }

        public CoinType()
        {
        }

        public CoinType(string name, byte value, long points) : base(name, value) => Points = points;
    }
}
