namespace GamaEdtech.Common.DataAccess.Query
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class OrderBy<TEntity>
    {
        public OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> expression) => Expression = expression;

        public OrderBy([NotNull] string columName, bool reverse) => Expression = GetOrderBy(columName, reverse);

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> Expression { get; private set; }

        private static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string columnName, bool reverse)
        {
            var typeQueryable = typeof(IQueryable<TEntity>);
            var argQueryable = System.Linq.Expressions.Expression.Parameter(typeQueryable, "p");
            var outerExpression = System.Linq.Expressions.Expression.Lambda(argQueryable, argQueryable);

            var entityType = typeof(TEntity);
            var arg = System.Linq.Expressions.Expression.Parameter(entityType, "x");

            Expression expression = arg;
            var properties = columnName.Split('.');
            foreach (var propertyName in properties)
            {
                var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expression = System.Linq.Expressions.Expression.Property(expression, propertyInfo!);
                entityType = propertyInfo!.PropertyType;
            }

            var lambda = System.Linq.Expressions.Expression.Lambda(expression, arg);
            var methodName = reverse ? "OrderByDescending" : "OrderBy";

            var resultExp = System.Linq.Expressions.Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(TEntity), entityType],
                outerExpression.Body,
                System.Linq.Expressions.Expression.Quote(lambda));

            var finalLambda = System.Linq.Expressions.Expression.Lambda(resultExp, argQueryable);

            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }
    }
}
