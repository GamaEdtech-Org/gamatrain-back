namespace GamaEdtech.Common.DataAccess.Specification
{
    using System;
    using System.Linq.Expressions;

    using GamaEdtech.Common.Core.Extensions.Linq;

    public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : SpecificationBase<T>
    {
        private readonly ISpecification<T> left = left;
        private readonly ISpecification<T> right = right;

        public override Expression<Func<T, bool>> Expression() => left.Expression().And(right.Expression());
    }
}
