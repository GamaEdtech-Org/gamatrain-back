namespace GamaEdtech.Common.DataAccess.Entities
{
    using System;

    using Microsoft.EntityFrameworkCore;

    public interface IEntity<TEntity, TKey> : IIdentifiable<TKey>, IEntityTypeConfiguration<TEntity>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
    }
}
