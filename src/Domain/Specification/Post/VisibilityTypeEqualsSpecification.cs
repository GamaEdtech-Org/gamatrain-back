namespace GamaEdtech.Domain.Specification.Post
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    public sealed class VisibilityTypeEqualsSpecification(VisibilityType visibilityType) : SpecificationBase<Post>
    {
        public override Expression<Func<Post, bool>> Expression() => (t) => t.VisibilityType == visibilityType;
    }
}
