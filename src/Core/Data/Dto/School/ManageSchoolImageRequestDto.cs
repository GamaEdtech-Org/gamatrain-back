namespace GamaEdtech.Data.Dto.School
{
    public sealed class ManageSchoolImageRequestDto
    {
        public long Id { get; set; }
        public long SchoolId { get; set; }
        public long? TagId { get; set; }
        public bool IsDefault { get; set; }
    }
}
