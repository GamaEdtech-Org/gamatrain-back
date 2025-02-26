namespace GamaEdtech.Common.DataAccess.Repositories
{
    using System;

    using GamaEdtech.Common.DataAccess.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class GenericEntityRepository<TEntity, TKey>(ILogger<IDataAccess> logger) : EntityRepositoryBase<DbContext, TEntity, TKey>(logger, null)
        where TEntity : class, IEntity<TEntity, TKey>, new()
        where TKey : IEquatable<TKey>
    {
    }
}
