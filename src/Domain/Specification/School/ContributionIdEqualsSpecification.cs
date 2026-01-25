namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class ContributionIdEqualsSpecification(long contributionId) : SpecificationBase<SchoolImage>
    {
        public override Expression<Func<SchoolImage, bool>> Expression() => (t) => t.ContributionId.Equals(contributionId);
    }
}
