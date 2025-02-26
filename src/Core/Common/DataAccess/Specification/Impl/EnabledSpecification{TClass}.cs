namespace GamaEdtech.Common.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAccess.Specification;

    public sealed class EnabledSpecification<TClass>(bool enabled) : SpecificationBase<TClass>
        where TClass : class, IEnablable
    {
        private readonly bool enabled = enabled;

        public override Expression<Func<TClass, bool>> Expression() => t => t.Enabled == enabled;
    }
}
