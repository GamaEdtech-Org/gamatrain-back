namespace GamaEdtech.Common.DataAccess.Paging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using GamaEdtech.Common.DataAccess.Entities;
    using GamaEdtech.Common.DataAccess.Query;
    using GamaEdtech.Common.DataAccess.UnitOfWork;

    public class DataPager<TEntity, TKey>(Lazy<IUnitOfWorkProvider> unitOfWorkProvider) : IDataPager<TEntity, TKey>
        where TEntity : class, IEntity<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {
        public DataPage<TEntity> Get(int pageNumber, int pageLength, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            using var uow = unitOfWorkProvider.Value.CreateUnitOfWork(false);
            var repository = uow.GetRepository<TEntity, TKey>();

            var startRow = (pageNumber - 1) * pageLength;
            var data = repository.GetPage(startRow, pageLength, includes: includes, orderBy: orderby?.Expression);
            var totalCount = repository.Count();

            return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        public async Task<DataPage<TEntity>> GetAsync(int pageNumber, int pageLength, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            using var uow = unitOfWorkProvider.Value.CreateUnitOfWork(false);
            var repository = uow.GetRepository<TEntity, TKey>();

            var startRow = (pageNumber - 1) * pageLength;
            var data = await repository.GetPageAsync(startRow, pageLength, includes: includes, orderBy: orderby?.Expression);
            var totalCount = await repository.CountAsync();

            return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        public DataPage<TEntity> Query(int pageNumber, int pageLength, [NotNull] Filter<TEntity> filter, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            using var uow = unitOfWorkProvider.Value.CreateUnitOfWork(false);
            var repository = uow.GetRepository<TEntity, TKey>();

            var startRow = (pageNumber - 1) * pageLength;
            var data = repository.QueryPage(startRow, pageLength, filter.Expression, includes: includes, orderBy: orderby?.Expression);
            var totalCount = repository.Count(filter.Expression);

            return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        public async Task<DataPage<TEntity>> QueryAsync(int pageNumber, int pageLength, [NotNull] Filter<TEntity> filter, OrderBy<TEntity>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            using var uow = unitOfWorkProvider.Value.CreateUnitOfWork(false);
            var repository = uow.GetRepository<TEntity, TKey>();

            var startRow = (pageNumber - 1) * pageLength;
            var data = await repository.QueryPageAsync(startRow, pageLength, filter.Expression, includes: includes, orderBy: orderby?.Expression);
            var totalCount = await repository.CountAsync(filter.Expression);

            return CreateDataPage(pageNumber, pageLength, data, totalCount);
        }

        private static DataPage<TEntity> CreateDataPage(int pageNumber, int pageLength, IEnumerable<TEntity>? data, long totalEntityCount) => new()
        {
            Data = data,
            TotalEntityCount = totalEntityCount,
            PageLength = pageLength,
            PageNumber = pageNumber,
        };
    }
}
