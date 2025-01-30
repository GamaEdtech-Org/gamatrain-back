namespace GamaEdtech.Backend.Data.Specification
{
    using Farsica.Framework.DataAccess.Specification;

    using System.Linq.Expressions;

    public sealed class UserIdEqualsSpecification<TClass, TKey>(TKey userId) : SpecificationBase<TClass>
        where TClass : IUserId<TKey>
        where TKey : IEquatable<TKey>
    {
        public override Expression<Func<TClass, bool>> Expression() => (t) => t.UserId.Equals(userId);
    }
}
