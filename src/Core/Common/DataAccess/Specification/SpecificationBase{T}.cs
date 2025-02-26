namespace GamaEdtech.Common.DataAccess.Specification
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using GamaEdtech.Common.Data;

    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        public virtual Func<IQueryable<T>, IOrderedQueryable<T>>? Order => null;

        public virtual PageFilter? PageFilter => null;

        public abstract Expression<Func<T, bool>> Expression();

        public bool IsSatisfiedBy(T candidate) => Expression().Compile()(candidate);
    }
}
