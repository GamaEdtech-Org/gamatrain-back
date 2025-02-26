namespace GamaEdtech.Domain.Specification.Grade
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class BoardIdEqualsSpecification(int boardId) : SpecificationBase<Grade>
    {
        public override Expression<Func<Grade, bool>> Expression() => (t) => t.BoardId == boardId;
    }
}
