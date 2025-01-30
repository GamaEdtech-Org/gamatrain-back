namespace GamaEdtech.Backend.Data.Specification.School
{
    using System.Linq.Expressions;

    using Farsica.Framework.DataAccess.Specification;

    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.Enumeration;

    public sealed class SchoolTypeEqualsSpecification(SchoolType schoolType) : SpecificationBase<School>
    {
        public override Expression<Func<School, bool>> Expression() => (t) => t.SchoolType == schoolType;
    }
}
