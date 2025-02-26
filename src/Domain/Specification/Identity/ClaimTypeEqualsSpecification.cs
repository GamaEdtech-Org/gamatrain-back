namespace GamaEdtech.Domain.Specification.Identity
{
    using GamaEdtech.Common.DataAccess.Specification;

    using System.Linq.Expressions;
    using GamaEdtech.Domain.Entity.Identity;

    public sealed class ClaimTypeEqualsSpecification(string claimType) : SpecificationBase<ApplicationUserClaim>
    {
        public override Expression<Func<ApplicationUserClaim, bool>> Expression() => (t) => t.ClaimType == claimType;
    }
}
