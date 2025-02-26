namespace GamaEdtech.Domain.Specification.Subject
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class GradeIdEqualsSpecification(int gradeId) : SpecificationBase<Subject>
    {
        public override Expression<Func<Subject, bool>> Expression() => (t) => t.Grades!.Any(g => g.Id == gradeId);
    }
}
