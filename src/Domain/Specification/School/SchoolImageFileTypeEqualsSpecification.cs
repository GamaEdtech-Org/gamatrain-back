namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    public sealed class SchoolImageFileTypeEqualsSpecification(FileType fileType) : SpecificationBase<SchoolImage>
    {
        public override Expression<Func<SchoolImage, bool>> Expression() => (t) => t.FileType == fileType;
    }
}
