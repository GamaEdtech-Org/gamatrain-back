namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class FileProviderType : Enumeration<byte>
    {
        [Display]
        public static readonly FileProviderType Local = new(nameof(Local), 0);

        [Display]
        public static readonly FileProviderType Azure = new(nameof(Azure), 1);

        public FileProviderType()
        {
        }

        public FileProviderType(string name, byte value) : base(name, value)
        {
        }
    }
}
