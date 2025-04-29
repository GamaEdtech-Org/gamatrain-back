namespace GamaEdtech.Common.DataAccess.Entities
{
    using Microsoft.AspNetCore.Identity;

    public interface ICreationableEntity<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey CreationUserId { get; set; }
        TUser CreationUser { get; set; }
        DateTimeOffset CreationDate { get; set; }
    }
}
