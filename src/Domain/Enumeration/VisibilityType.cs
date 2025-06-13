namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class VisibilityType : Enumeration<VisibilityType, byte>
    {
        [Display]
        public static readonly VisibilityType General = new(nameof(General), 0);

        [Display]
        public static readonly VisibilityType Premium = new(nameof(Premium), 1);

        [Display]
        public static readonly VisibilityType Private = new(nameof(Private), 2);

        public VisibilityType()
        {
        }

        public VisibilityType(string name, byte value) : base(name, value)
        {
        }
    }
}
