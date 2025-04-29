namespace GamaEdtech.Domain.Specification.Post
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class TagIncludedSpecification(int tagId) : SpecificationBase<Post>
    {
        public override Expression<Func<Post, bool>> Expression() => (t) => t.PostTags != null && t.PostTags.Any(p => p.TagId == tagId);
    }
}
