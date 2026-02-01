namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ChangeFrequency : Enumeration<ChangeFrequency, byte>
    {
        [Display]
        public static readonly ChangeFrequency Always = new(nameof(Always), 0);

        [Display]
        public static readonly ChangeFrequency Hourly = new(nameof(Hourly), 1);

        [Display]
        public static readonly ChangeFrequency Daily = new(nameof(Daily), 2);

        [Display]
        public static readonly ChangeFrequency Weekly = new(nameof(Weekly), 3);

        [Display]
        public static readonly ChangeFrequency Monthly = new(nameof(Monthly), 4);

        [Display]
        public static readonly ChangeFrequency Yearly = new(nameof(Yearly), 5);

        [Display]
        public static readonly ChangeFrequency Never = new(nameof(Never), 6);

        public ChangeFrequency()
        {
        }

        public ChangeFrequency(string name, byte value) : base(name, value)
        {
        }
    }
}
