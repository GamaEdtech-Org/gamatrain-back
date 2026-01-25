namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    public sealed class HasImageSpecification(bool hasImage) : SpecificationBase<School>
    {
        public override Expression<Func<School, bool>> Expression() => (t) => hasImage && t.DefaultImageId != null;
    }
}
