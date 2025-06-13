namespace GamaEdtech.Domain.Specification.Post
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class SlugEqualsSpecification(string slug) : SpecificationBase<Post>
    {
        public override Expression<Func<Post, bool>> Expression() => (t) => t.Slug == slug;
    }
}
