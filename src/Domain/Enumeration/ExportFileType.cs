namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ExportFileType : Enumeration<ExportFileType, byte>
    {
        [Display]
        public static readonly ExportFileType Pdf = new(nameof(Pdf), 0, ".pdf");

        [Display]
        public static readonly ExportFileType Word = new(nameof(Word), 1, ".docx");

        [Display]
        public static readonly ExportFileType PowerPoint = new(nameof(PowerPoint), 2, ".pptx");


        public string Extension { get; }

        public ExportFileType()
        {
        }

        public ExportFileType(string name, byte value, string extension)
            : base(name, value) => Extension = extension;
    }
}
