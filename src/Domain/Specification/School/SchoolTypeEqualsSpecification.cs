namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    public sealed class SchoolTypeEqualsSpecification(SchoolType schoolType) : SpecificationBase<School>
    {
        public override Expression<Func<School, bool>> Expression() => (t) => t.SchoolType == schoolType;
    }
}
