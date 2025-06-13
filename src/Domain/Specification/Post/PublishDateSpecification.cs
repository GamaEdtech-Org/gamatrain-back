namespace GamaEdtech.Domain.Specification.Post
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class PublishDateSpecification : SpecificationBase<Post>
    {
        public override Expression<Func<Post, bool>> Expression() => (t) => t.PublishDate <= DateTimeOffset.UtcNow;
    }
}
