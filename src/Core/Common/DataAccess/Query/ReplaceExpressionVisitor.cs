namespace GamaEdtech.Common.DataAccess.Query
{
    using System.Linq.Expressions;

    public class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
    {
        private readonly Expression oldValue = oldValue;
        private readonly Expression newValue = newValue;

        public override Expression? Visit(Expression? node) => node == oldValue ? newValue : base.Visit(node);
    }
}
