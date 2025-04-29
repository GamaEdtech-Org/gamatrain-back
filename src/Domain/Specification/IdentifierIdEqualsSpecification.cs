namespace GamaEdtech.Domain.Specification
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class IdentifierIdEqualsSpecification<TClass>(long identifierId) : SpecificationBase<TClass>
        where TClass : IIdentifierId
    {
        public override Expression<Func<TClass, bool>> Expression() => (t) => t.IdentifierId.Equals(identifierId);
    }
}
