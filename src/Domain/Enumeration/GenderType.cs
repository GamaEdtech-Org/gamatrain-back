namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;

    public sealed class GenderType : Enumeration<GenderType, byte>
    {
        public static readonly GenderType Male = new(nameof(Male), 1);
        public static readonly GenderType Female = new(nameof(Female), 2);
        public static readonly GenderType Other = new(nameof(Other), 3);

        public GenderType() { }
        public GenderType(string name, byte value) : base(name, value) { }
    }
}
