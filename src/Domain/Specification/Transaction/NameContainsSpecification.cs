namespace GamaEdtech.Domain.Specification.Transaction
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class NameContainsSpecification(string name) : SpecificationBase<Transaction>
    {
        public override Expression<Func<Transaction, bool>> Expression() => (t) => t.User!.FirstName!.Contains(name) || t.User.LastName!.Contains(name);
    }
}
