namespace GamaEdtech.Domain.Specification.Identity
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity.Identity;

    public sealed class HasReferralSpecification(bool hasReferral) : SpecificationBase<ApplicationUser>
    {
        public override Expression<Func<ApplicationUser, bool>> Expression() => (t) => hasReferral ? !string.IsNullOrEmpty(t.ReferralId) : string.IsNullOrEmpty(t.ReferralId);
    }
}
