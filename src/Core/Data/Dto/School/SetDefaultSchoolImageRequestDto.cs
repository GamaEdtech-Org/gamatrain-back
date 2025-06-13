namespace GamaEdtech.Data.Dto.School
{
    public sealed class SetDefaultSchoolImageRequestDto
    {
        public required long Id { get; set; }
        public required long SchoolId { get; set; }
    }
}
