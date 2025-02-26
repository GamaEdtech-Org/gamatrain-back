namespace GamaEdtech.Common.DataAccess.Paging
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAccess.Query;

    [DataAnnotation.Injectable]
    public interface IDataPager<TEntity, TKey>
        where TEntity : class, IEntity<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {
        DataPage<TEntity> Get(int pageNumber, int pageLength, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        DataPage<TEntity> Query(int pageNumber, int pageLength, Filter<TEntity> filter, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<DataPage<TEntity>> GetAsync(int pageNumber, int pageLength, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<DataPage<TEntity>> QueryAsync(int pageNumber, int pageLength, Filter<TEntity> filter, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);
    }
}
