namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class FileType : Enumeration<FileType, byte>
    {
        [Display]
        public static readonly FileType SimpleImage = new(nameof(SimpleImage), 0);

        [Display]
        public static readonly FileType Tour360 = new(nameof(Tour360), 1);

        public FileType()
        {
        }

        public FileType(string name, byte value) : base(name, value)
        {
        }
    }
}
