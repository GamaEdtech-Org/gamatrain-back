namespace GamaEdtech.Domain.Specification.Transaction
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class EmailEqualsSpecification(string email) : SpecificationBase<Transaction>
    {
        public override Expression<Func<Transaction, bool>> Expression()
        {
            email = email.ToUpperInvariant();
            return (t) => t.User!.NormalizedEmail == email;
        }
    }
}
