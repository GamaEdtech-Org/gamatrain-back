namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class SchoolType : Enumeration<byte>
    {
        [Display]
        public static readonly SchoolType Public = new(nameof(Public), 0);

        [Display]
        public static readonly SchoolType Private = new(nameof(Private), 1);

        [Display]
        public static readonly SchoolType Religious = new(nameof(Religious), 2);

        [Display]
        public static readonly SchoolType FirstNation = new(nameof(FirstNation), 3);

        [Display]
        public static readonly SchoolType PrivateNonProfit = new(nameof(PrivateNonProfit), 4);

        [Display]
        public static readonly SchoolType Government = new(nameof(Government), 5);

        [Display]
        public static readonly SchoolType Community = new(nameof(Community), 6);

        public SchoolType()
        {
        }

        public SchoolType(string name, byte value) : base(name, value)
        {
        }
    }
}
