namespace GamaEdtech.Domain.Specification.Post
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class TitleContainsSpecification(string title) : SpecificationBase<Post>
    {
        public override Expression<Func<Post, bool>> Expression() => (t) => t.Title!.Contains(title);
    }
}
