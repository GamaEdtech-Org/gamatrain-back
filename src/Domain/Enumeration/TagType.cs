namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class TagType : Enumeration<byte>
    {
        [Display]
        public static readonly TagType Facility = new(nameof(Facility), 0);

        [Display]
        public static readonly TagType Service = new(nameof(Service), 1);

        [Display]
        public static readonly TagType Feature = new(nameof(Feature), 2);

        public TagType()
        {
        }

        public TagType(string name, byte value) : base(name, value)
        {
        }
    }
}
