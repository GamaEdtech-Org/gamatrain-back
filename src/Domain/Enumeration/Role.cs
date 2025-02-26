namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    using System.Collections;

    public sealed class Role : FlagsEnumeration<Role>
    {
        [Display]
        public static readonly Role Admin = new(1);

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
