namespace GamaEdtech.Data.Dto.School
{
    public sealed class SchoolCommentReactionRequestDto
    {
        public required long SchoolId { get; set; }
        public required long CommentId { get; set; }
    }
}
