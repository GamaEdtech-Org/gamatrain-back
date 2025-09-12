namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class Currency : Enumeration<Currency, byte>
    {
        [Display]
        public static readonly Currency SOL = new(nameof(SOL), 0);

        [Display]
        public static readonly Currency USDC = new(nameof(USDC), 1);

        [Display]
        public static readonly Currency GET = new(nameof(GET), 2);

        [Display]
        public static readonly Currency USDT = new(nameof(USDT), 3);

        public Currency()
        {
        }

        public Currency(string name, byte value) : base(name, value)
        {
        }
    }
}
