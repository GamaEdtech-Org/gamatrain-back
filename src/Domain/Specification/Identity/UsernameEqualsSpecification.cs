namespace GamaEdtech.Domain.Specification.Identity
{
    using GamaEdtech.Common.DataAccess.Specification;

    using System.Linq.Expressions;
    using GamaEdtech.Domain.Entity.Identity;

    public sealed class UsernameEqualsSpecification(string username) : SpecificationBase<ApplicationUser>
    {
        public override Expression<Func<ApplicationUser, bool>> Expression()
        {
            username = username.ToUpperInvariant();
            return (t) => t.NormalizedUserName == username;
        }
    }
}
