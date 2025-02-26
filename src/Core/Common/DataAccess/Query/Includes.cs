namespace GamaEdtech.Common.DataAccess.Query
{
    using System;
    using System.Linq;

    public class Includes<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> expression)
    {
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Expression { get; private set; } = expression;
    }
}
