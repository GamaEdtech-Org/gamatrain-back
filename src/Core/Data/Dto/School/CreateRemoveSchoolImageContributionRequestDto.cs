namespace GamaEdtech.Data.Dto.School
{
    public sealed class CreateRemoveSchoolImageContributionRequestDto
    {
        public long SchoolId { get; set; }
        public long ImageId { get; set; }
        public int CreationUserId { get; set; }
        public string? Description { get; set; }
    }
}
