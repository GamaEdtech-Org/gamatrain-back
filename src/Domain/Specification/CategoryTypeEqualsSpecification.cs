namespace GamaEdtech.Domain.Specification
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    public sealed class CategoryTypeEqualsSpecification<TClass>(CategoryType categoryType) : SpecificationBase<TClass>
        where TClass : ICategoryType
    {
        public override Expression<Func<TClass, bool>> Expression() => (t) => t.CategoryType!.Equals(categoryType);
    }
}
