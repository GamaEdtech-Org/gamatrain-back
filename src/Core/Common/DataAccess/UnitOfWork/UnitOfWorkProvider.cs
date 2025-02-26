namespace GamaEdtech.Common.DataAccess.UnitOfWork
{
    using System;

    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using GamaEdtech.Common.DataAccess.Context;
    using GamaEdtech.Common.DataAccess;

    [ServiceLifetime(ServiceLifetime.Scoped)]
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly ILogger<IDataAccess> logger;
        private readonly IServiceProvider serviceProvider;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public UnitOfWorkProvider()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
        }

        public UnitOfWorkProvider(ILogger<IDataAccess> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public IUnitOfWork CreateUnitOfWork(bool trackChanges = true, bool enableLogging = false)
        {
            var context = serviceProvider.GetRequiredService<IEntityContext>() as DbContext;

            if (!trackChanges)
            {
                context!.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return new UnitOfWork<DbContext>(context!, serviceProvider, logger);
        }

        public IUnitOfWork CreateUnitOfWork<T>(bool trackChanges = true, bool enableLogging = false)
            where T : DbContext
        {
            var context = serviceProvider.GetRequiredService<T>();

            if (!trackChanges)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return new UnitOfWork<T>(context, serviceProvider, logger);
        }
    }
}
