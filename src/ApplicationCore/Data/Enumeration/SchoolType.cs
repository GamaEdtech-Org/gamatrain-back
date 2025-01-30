namespace GamaEdtech.Backend.Data.Enumeration
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;

    public sealed class SchoolType : Enumeration<byte>
    {
        [Display(Name = "Public")]
        public static readonly SchoolType Public = new(nameof(Public), 0);

        [Display(Name = "Private")]
        public static readonly SchoolType Private = new(nameof(Private), 1);

        public SchoolType()
        {
        }

        public SchoolType(string name, byte value) : base(name, value)
        {
        }
    }
}
