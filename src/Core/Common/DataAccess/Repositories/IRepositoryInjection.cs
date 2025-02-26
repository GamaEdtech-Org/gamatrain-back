namespace GamaEdtech.Common.DataAccess.Repositories
{
    using Microsoft.EntityFrameworkCore;

    public interface IRepositoryInjection<TContext>
        where TContext : DbContext
    {
        IRepositoryInjection<TContext> SetContext(TContext context);
    }
}
