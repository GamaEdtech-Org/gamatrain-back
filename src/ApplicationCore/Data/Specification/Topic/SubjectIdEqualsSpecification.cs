namespace GamaEdtech.Backend.Data.Specification.Topic
{
    using System.Linq.Expressions;

    using Farsica.Framework.DataAccess.Specification;

    using GamaEdtech.Backend.Data.Entity;

    public sealed class SubjectIdEqualsSpecification(int subjectId) : SpecificationBase<Topic>
    {
        public override Expression<Func<Topic, bool>> Expression() => (t) => t.Subjects!.Any(s => s.Id == subjectId);
    }
}
