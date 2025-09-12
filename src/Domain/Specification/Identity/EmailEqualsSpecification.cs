namespace GamaEdtech.Domain.Specification.Identity
{
    using GamaEdtech.Common.DataAccess.Specification;

    using System.Linq.Expressions;
    using GamaEdtech.Domain.Entity.Identity;

    public sealed class EmailEqualsSpecification(string email) : SpecificationBase<ApplicationUser>
    {
        public override Expression<Func<ApplicationUser, bool>> Expression()
        {
            email = email.ToUpperInvariant();
            return (t) => t.NormalizedEmail == email;
        }
    }
}
