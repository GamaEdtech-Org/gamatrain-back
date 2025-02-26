namespace GamaEdtech.Common.Data.Enumeration
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Core;

#pragma warning disable S4035 // Classes implementing "IEquatable<T>" should be sealed
    public abstract class Enumeration<T> : IComparable, IEquatable<Enumeration<T>>, IComparable<Enumeration<T>>
#pragma warning restore S4035 // Classes implementing "IEquatable<T>" should be sealed
        where T : IEquatable<T>, IComparable<T>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [ExcludeFromCodeCoverage]
        protected Enumeration()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
        }

        protected Enumeration([NotNull] string name, T value)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; set; }

        public T Value { get; set; }

        public string? LocalizedDisplayName => Globals.GetLocalizedDisplayName(GetType().GetField(Name));

        public string? LocalizedShortName => Globals.GetLocalizedShortName(GetType().GetField(Name));

        public string? LocalizedDescription => Globals.GetLocalizedDescription(GetType().GetField(Name));

        public string? LocalizedPromt => Globals.GetLocalizedPromt(GetType().GetField(Name));

        public string? LocalizedGroupName => Globals.GetLocalizedGroupName(GetType().GetField(Name));

        public static bool operator ==(Enumeration<T>? left, Enumeration<T>? right) => Equals(left, right);

        public static bool operator !=(Enumeration<T>? left, Enumeration<T>? right) => !Equals(left, right);

        public override string? ToString() => Name;

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator <(Enumeration<T> left, Enumeration<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;

        public static bool operator <=(Enumeration<T> left, Enumeration<T> right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(Enumeration<T> left, Enumeration<T> right) => left is not null && left.CompareTo(right) > 0;

        public static bool operator >=(Enumeration<T> left, Enumeration<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;

        public override bool Equals(object? obj) => obj is Enumeration<T> && Equals(obj as Enumeration<T>);

        public bool Equals(Enumeration<T>? other) => other is not null && (ReferenceEquals(this, other) || Value.Equals(other.Value));

#pragma warning disable CA1062 // Validate arguments of public methods
        public int CompareTo(Enumeration<T>? other) => Value.CompareTo(other!.Value);

        public int CompareTo(object? obj) => Value.CompareTo((obj as Enumeration<T>)!.Value);
#pragma warning restore CA1062 // Validate arguments of public methods
    }
}
