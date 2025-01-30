namespace GamaEdtech.Backend.Data.Specification.Identity
{
    using GamaEdtech.Backend.Data.Entity.Identity;

    using Farsica.Framework.DataAccess.Specification;

    using System.Linq.Expressions;

    public sealed class ClaimTypeEqualsSpecification(string claimType) : SpecificationBase<ApplicationUserClaim>
    {
        public override Expression<Func<ApplicationUserClaim, bool>> Expression() => (t) => t.ClaimType == claimType;
    }
}
