namespace GamaEdtech.Domain.Specification.Topic
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class SubjectIdEqualsSpecification(int subjectId) : SpecificationBase<Topic>
    {
        public override Expression<Func<Topic, bool>> Expression() => (t) => t.Subjects!.Any(s => s.Id == subjectId);
    }
}
