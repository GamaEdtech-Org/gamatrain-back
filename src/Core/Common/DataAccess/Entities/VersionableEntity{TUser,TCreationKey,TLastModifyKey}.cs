namespace GamaEdtech.Common.DataAccess.Entities
{
    using System;

    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.AspNetCore.Identity;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
    public abstract class VersionableEntity<TUser, TCreationKey, TLastModifyKey> : IVersionableEntity<TUser, TCreationKey, TLastModifyKey>
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
        where TUser : IdentityUser<TCreationKey>
        where TCreationKey : IEquatable<TCreationKey>
    {
        [Column(nameof(CreationDate), Data.DataType.DateTimeOffset)]
        [Required]
        public virtual DateTimeOffset CreationDate { get; set; }

        [Column(nameof(CreationUserId))]
        [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public virtual TCreationKey CreationUserId { get; set; }

        public TUser CreationUser { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Column(nameof(LastModifyDate), Data.DataType.DateTimeOffset)]
        public virtual DateTimeOffset? LastModifyDate { get; set; }

        [Column(nameof(LastModifyUserId))]
        public virtual TLastModifyKey? LastModifyUserId { get; set; }

        public TUser? LastModifyUser { get; set; }
    }
}
