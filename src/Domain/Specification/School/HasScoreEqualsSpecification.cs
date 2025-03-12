namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class HasScoreEqualsSpecification(bool hasScore) : SpecificationBase<School>
    {
        public override Expression<Func<School, bool>> Expression() => (t) => hasScore ? (t.Comments != null && t.Comments.Any()) : (t.Comments == null || !t.Comments.Any());
    }
}
