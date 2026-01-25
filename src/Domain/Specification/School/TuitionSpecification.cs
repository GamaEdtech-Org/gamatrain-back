namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class TuitionSpecification(decimal? start, decimal? end) : SpecificationBase<School>
    {
        public override Expression<Func<School, bool>> Expression() => (t) => (start == null || t.Tuition >= start.Value) && (end == null || t.Tuition <= end.Value);
    }
}
