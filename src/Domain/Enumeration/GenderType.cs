namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class GenderType : Enumeration<GenderType, byte>
    {
        [Display]
        public static readonly GenderType Male = new(nameof(Male), 0);

        [Display]
        public static readonly GenderType Female = new(nameof(Female), 1);

        [Display]
        public static readonly GenderType Other = new(nameof(Other), 2);

        public GenderType()
        {
        }

        public GenderType(string name, byte value) : base(name, value)
        {
        }
    }
}
