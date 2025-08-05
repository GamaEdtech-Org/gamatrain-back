namespace GamaEdtech.Data.Dto.School
{
    public sealed class ManageSchoolCommentContributionRequestDto
    {
        public long? Id { get; set; }
        public long SchoolId { get; set; }
        public int UserId { get; set; }

        public required SchoolCommentContributionDto CommentContribution { get; set; }
    }
}
