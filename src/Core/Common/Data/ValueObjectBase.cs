namespace GamaEdtech.Common.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ValueObjectBase
    {
        public static bool operator ==(ValueObjectBase? left, ValueObjectBase? right) => EqualOperator(left, right);

        public static bool operator !=(ValueObjectBase? left, ValueObjectBase? right) => NotEqualOperator(left, right);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            if (obj is not ValueObjectBase other)
            {
                return false;
            }

            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^
                    otherValues.Current is null)
                {
                    return false;
                }

                if (thisValues.Current is not null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode() => GetAtomicValues()
             .Select(t => t is not null ? t.GetHashCode() : 0)
             .Aggregate((t1, t2) => t1 ^ t2);

        protected static bool EqualOperator(ValueObjectBase? left, ValueObjectBase? right) => !(left is null ^ right is null) && (left is null || left.Equals(right));

        protected static bool NotEqualOperator(ValueObjectBase? left, ValueObjectBase? right) => !EqualOperator(left, right);

        protected abstract IEnumerable<object> GetAtomicValues();
    }
}
