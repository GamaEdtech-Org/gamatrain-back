namespace GamaEdtech.Common.DataAccess.Specification
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) => new AndSpecification<T>(left, right);

        public static ISpecification<T> AndNot<T>(this ISpecification<T> left, ISpecification<T> right) => new AndNotSpecification<T>(left, right);

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) => new OrSpecification<T>(left, right);

        public static ISpecification<T> OrNot<T>(this ISpecification<T> left, ISpecification<T> right) => new OrNotSpecification<T>(left, right);

        public static ISpecification<T> Not<T>(this ISpecification<T> specification) => new NotSpecification<T>(specification);
    }
}
