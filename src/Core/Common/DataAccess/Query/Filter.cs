namespace GamaEdtech.Common.DataAccess.Query
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public class Filter<TEntity>(Expression<Func<TEntity, bool>> expression)
    {
        public Expression<Func<TEntity, bool>> Expression { get; private set; } = expression;

        public void AddExpression([NotNull] Expression<Func<TEntity, bool>> newExpression)
        {
            Expression ??= newExpression ?? throw new ArgumentNullException(nameof(newExpression));

            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TEntity));

            var leftVisitor = new ReplaceExpressionVisitor(newExpression.Parameters[0], parameter);
            var left = leftVisitor.Visit(newExpression.Body);

            var rightVisitor = new ReplaceExpressionVisitor(Expression.Parameters[0], parameter);
            var right = rightVisitor.Visit(Expression.Body);

            Expression = System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(System.Linq.Expressions.Expression.AndAlso(left!, right!), parameter);
        }
    }
}
