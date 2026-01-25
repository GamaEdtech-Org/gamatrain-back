namespace GamaEdtech.Domain.Specification.Post
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class PublishDateEqualsSpecification(DateOnly date) : SpecificationBase<Post>
    {
        public override Expression<Func<Post, bool>> Expression()
        {
            var start = new DateTimeOffset(date, new TimeOnly(), TimeSpan.Zero);
            var end = new DateTimeOffset(date, new TimeOnly(23, 59, 59, 999), TimeSpan.Zero);
            return (t) => t.PublishDate >= start && t.PublishDate <= end;
        }
    }
}
