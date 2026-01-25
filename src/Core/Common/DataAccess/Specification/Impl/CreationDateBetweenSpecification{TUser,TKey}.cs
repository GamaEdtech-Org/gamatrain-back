namespace GamaEdtech.Common.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAccess.Specification;

    public sealed class CreationDateBetweenSpecification<T>(DateTimeOffset? start, DateTimeOffset? end) : SpecificationBase<T>
        where T : class, ICreationDate
    {
        public override Expression<Func<T, bool>> Expression() => t => (start == null || t.CreationDate >= start) && (end == null || t.CreationDate <= end);
    }
}
