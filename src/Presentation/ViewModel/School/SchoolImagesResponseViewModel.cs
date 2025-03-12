namespace GamaEdtech.Presentation.ViewModel.School
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class SchoolImagesResponseViewModel
    {
        public long Id { get; set; }
        public string? CreationUser { get; set; }
        public string? CreationUserAvatar { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string? FileId { get; set; }
        public FileType? FileType { get; set; }
        public int SchoolId { get; set; }
        public string? SchoolName { get; set; }
    }
}
