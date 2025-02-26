namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class SchoolIdEqualsSpecification<TClass>(int schoolId) : SpecificationBase<TClass>
        where TClass : ISchoolId
    {
        public override Expression<Func<TClass, bool>> Expression() => (t) => t.SchoolId.Equals(schoolId);
    }
}
