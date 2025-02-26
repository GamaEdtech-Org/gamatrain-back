namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class StateIdEqualsSpecification(int stateId) : SpecificationBase<School>
    {
        public override Expression<Func<School, bool>> Expression() => (t) => t.StateId == stateId;
    }
}
