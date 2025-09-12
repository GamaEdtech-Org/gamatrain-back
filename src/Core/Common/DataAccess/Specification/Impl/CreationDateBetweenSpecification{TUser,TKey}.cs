namespace GamaEdtech.Common.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAccess.Specification;

    using Microsoft.AspNetCore.Identity;

    public sealed class CreationDateBetweenSpecification<TClass, TUser, TKey>(DateTimeOffset? start, DateTimeOffset? end) : SpecificationBase<TClass>
        where TClass : class, ICreationableEntity<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public override Expression<Func<TClass, bool>> Expression() => t => (start == null || t.CreationDate >= start) && (end == null || t.CreationDate <= end);
    }
}
