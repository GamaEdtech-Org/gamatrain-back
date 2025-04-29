namespace GamaEdtech.Common.Data.Enumeration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class EnumerationExtensions
    {
        public static IEnumerable<TEnum?>? GetAll<TEnum, TKey>()
            where TEnum : Enumeration<TEnum, TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            var fields = typeof(TEnum).GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            return fields.Select(t => t.GetValue(null) as TEnum);
        }

        public static IEnumerable<object?>? GetAll([NotNull] Type type)
        {
            var fields = type.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            return fields.Select(t => t.GetValue(null));
        }

        public static IEnumerable<string?>? GetNames<TEnum, TKey>()
            where TEnum : Enumeration<TEnum, TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey> => GetNames(typeof(TEnum));

        public static IEnumerable<string?>? GetNames([NotNull] Type type)
        {
            var fields = type.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            return fields.Select(t => t.GetValue(null)?.ToString());
        }

        public static bool TryGetFromNameOrValue<TEnum, TKey>(this string? nameOrValue, out TEnum? enumeration)
            where TEnum : Enumeration<TEnum, TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey> => TryParse<TEnum, TKey>(t => t.Name.Equals(nameOrValue, StringComparison.OrdinalIgnoreCase), out enumeration) ||
                   (int.TryParse(nameOrValue, out var value) && TryParse<TEnum, TKey>(t => t.Value.CompareTo((TKey)(object)value) == 0, out enumeration));

        public static TEnum ToEnumeration<TEnum, TKey>(this TKey value)
            where TEnum : Enumeration<TEnum, TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            var item = GetAll<TEnum, TKey>()?.FirstOrDefault(t => value.Equals(t!.Value));
            return item is null ? throw new ArgumentOutOfRangeException(nameof(value)) : item;
        }

        public static TEnum ToEnumeration<TEnum, TKey>([NotNull] this string name)
            where TEnum : Enumeration<TEnum, TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            var item = GetAll<TEnum, TKey>()?.FirstOrDefault(t => t?.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
            return item is null ? throw new ArgumentOutOfRangeException(nameof(name)) : item;
        }

        public static PropertyBuilder<TEnum?> OwnEnumeration<TEntity, TEnum, TKey>([NotNull] this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TEnum?>> property)
            where TEntity : class
            where TEnum : Enumeration<TEnum, TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey> => builder.Property(property).HasConversion(t => t!.Value, t => t.ToEnumeration<TEnum, TKey>());

        private static bool TryParse<TEnum, TKey>([NotNull] Func<TEnum, bool> predicate, out TEnum? enumeration)
            where TEnum : Enumeration<TEnum, TKey>
            where TKey : IEquatable<TKey>, IComparable<TKey>
        {
            enumeration = GetAll<TEnum, TKey>()?.FirstOrDefault(predicate!);
            return enumeration is not null;
        }
    }
}
