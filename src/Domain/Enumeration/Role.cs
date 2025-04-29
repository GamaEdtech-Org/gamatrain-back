namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    using System.Collections;

    public sealed class Role : FlagsEnumeration<Role>
    {
        [Display]
        public static readonly Role Admin = new(1);

        [Display]
        public static readonly Role Teacher = new(2);

        [Display]
        public static readonly Role Student = new(3);

        public Role()
        {
        }

        public Role(BitArray value)
            : base(value)
        {
        }

        public Role(byte[] value)
            : base(value)
        {
        }

        public Role(int index)
            : base(index)
        {
        }
    }
}
