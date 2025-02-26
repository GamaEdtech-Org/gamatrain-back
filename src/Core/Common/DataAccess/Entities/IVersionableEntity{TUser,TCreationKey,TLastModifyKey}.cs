namespace GamaEdtech.Common.DataAccess.Entities
{
    using System;

    using Microsoft.AspNetCore.Identity;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public interface IVersionableEntity<TUser, TCreationKey, TLastModifyKey>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
        where TUser : IdentityUser<TCreationKey>
        where TCreationKey : IEquatable<TCreationKey>
    {
        DateTimeOffset CreationDate { get; set; }

        TCreationKey CreationUserId { get; set; }

        TUser CreationUser { get; set; }

        DateTimeOffset? LastModifyDate { get; set; }

        TLastModifyKey? LastModifyUserId { get; set; }

        TUser? LastModifyUser { get; set; }
    }
}
