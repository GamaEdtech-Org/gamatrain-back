namespace GamaEdtech.Domain.Specification
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    public sealed class StatusEqualsSpecification<TClass>(Status status) : SpecificationBase<TClass>
        where TClass : IStatus
    {
        public override Expression<Func<TClass, bool>> Expression() => (t) => t.Status!.Equals(status);
    }
}
