namespace GamaEdtech.Common.DataAccess.Entities
{
    using System;

    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAnnotation.Schema;

    using Microsoft.AspNetCore.Identity;

    public abstract class CreationableEntity<TUser, TKey> : ICreationableEntity<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        [Column(nameof(CreationDate), Data.DataType.DateTimeOffset)]
        [Required]
        public virtual DateTimeOffset CreationDate { get; set; }

        [Column(nameof(CreationUserId))]
        [Required]
        public virtual TKey CreationUserId { get; set; }
        public TUser CreationUser { get; set; }
    }
}
