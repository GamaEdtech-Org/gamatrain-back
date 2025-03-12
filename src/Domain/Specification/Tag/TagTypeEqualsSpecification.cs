namespace GamaEdtech.Domain.Specification.Tag
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    public sealed class TagTypeEqualsSpecification(TagType tagType) : SpecificationBase<Tag>
    {
        public override Expression<Func<Tag, bool>> Expression() => (t) => t.TagType == tagType;
    }
}
