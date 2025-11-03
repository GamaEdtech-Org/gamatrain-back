namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ImageFileType : Enumeration<ImageFileType, byte>
    {
        [Display]
        public static readonly ImageFileType SimpleImage = new(nameof(SimpleImage), 0);

        [Display]
        public static readonly ImageFileType Tour360 = new(nameof(Tour360), 1);

        public ImageFileType()
        {
        }

        public ImageFileType(string name, byte value) : base(name, value)
        {
        }
    }
}
