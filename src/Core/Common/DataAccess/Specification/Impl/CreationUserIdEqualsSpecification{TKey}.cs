namespace GamaEdtech.Common.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAccess.Specification;

    using Microsoft.AspNetCore.Identity;

    public sealed class CreationUserIdEqualsSpecification<TClass, TUser, TKey>(TKey creationUserId) : SpecificationBase<TClass>
        where TClass : class, ICreationableEntity<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public override Expression<Func<TClass, bool>> Expression() => t => t.CreationUserId.Equals(creationUserId);
    }
}
