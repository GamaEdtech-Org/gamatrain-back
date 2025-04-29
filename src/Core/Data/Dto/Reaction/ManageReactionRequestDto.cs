namespace GamaEdtech.Data.Dto.Reaction
{
    using GamaEdtech.Domain.Enumeration;

    public sealed class ManageReactionRequestDto
    {
        public required CategoryType CategoryType { get; set; }
        public long IdentifierId { get; set; }
        public int CreationUserId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool IsLike { get; set; }
    }
}
