namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class FileProviderType : Enumeration<FileProviderType, byte>
    {
        [Display]
        public static readonly FileProviderType Local = new(nameof(Local), 0);

        [Display]
        public static readonly FileProviderType Azure = new(nameof(Azure), 1);

        [Display]
        public static readonly FileProviderType Youtube = new(nameof(Youtube), 2);

        public FileProviderType()
        {
        }

        public FileProviderType(string name, byte value) : base(name, value)
        {
        }
    }
}
