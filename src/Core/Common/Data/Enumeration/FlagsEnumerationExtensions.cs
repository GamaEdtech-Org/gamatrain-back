namespace GamaEdtech.Common.Data.Enumeration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class FlagsEnumerationExtensions
    {
        public static IEnumerable<string> GetNames<TEnum>(BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static)
            where TEnum : FlagsEnumeration<TEnum>, new() => typeof(TEnum)
                .GetFields(bindingFlags)
                .Select(f => f.Name);

        public static IEnumerable<string> GetNames<TEnum>(this TEnum enumFlag, BindingFlags bindingFlag = BindingFlags.Public | BindingFlags.Static)
            where TEnum : FlagsEnumeration<TEnum>, new() => GetKeyValues<TEnum>(bindingFlag).Where(t => enumFlag.HasFlags(t.Value)).Select(t => t.Key);

        public static IEnumerable<string> GetNames<TEnum>(this FlagsEnumeration<TEnum> enumFlag, BindingFlags bindingFlag = BindingFlags.Public | BindingFlags.Static)
            where TEnum : FlagsEnumeration<TEnum>, new() => GetKeyValues<TEnum>(bindingFlag).Where(t => enumFlag.HasFlags(t.Value)).Select(t => t.Key);

        public static IEnumerable<TEnum> GetValues<TEnum>(this FlagsEnumeration<TEnum> enumFlag, BindingFlags bindingFlag = BindingFlags.Public | BindingFlags.Static)
            where TEnum : FlagsEnumeration<TEnum>, new() => GetKeyValues<TEnum>(bindingFlag).Where(t => enumFlag.HasFlags(t.Value)).Select(t => t.Value);

        public static Dictionary<string, TEnum> GetKeyValues<TEnum>(BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static)
            where TEnum : FlagsEnumeration<TEnum>, new() => typeof(TEnum)
                .GetFields(bindingFlags)
                .ToDictionary(t => t.Name, t => (TEnum)t.GetValue(null)!);

        public static TEnum? FromName<TEnum>(this string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static)
            where TEnum : FlagsEnumeration<TEnum>, new() => typeof(TEnum).GetField(name, bindingFlags)?.GetValue(null) as TEnum;

        public static TEnum GetAll<TEnum>()
            where TEnum : FlagsEnumeration<TEnum>, new()
        {
            var count = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static).Length;
            return new TEnum { Bits = new BitArray(count, true) };
        }

        public static TEnum FromUniqueId<TEnum>(this string id)
            where TEnum : FlagsEnumeration<TEnum>, new()
        {
            var data = Convert.FromBase64String(id);
            using var compressedStream = new MemoryStream(data);
            using var outputStream = new MemoryStream();
            using var zipStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
            zipStream.CopyTo(outputStream);
            zipStream.Close();
            var bytes = outputStream.ToArray();

            return new TEnum { Bits = new BitArray(bytes) };
        }

        public static bool HasFlags<TEnum>(this TEnum left, TEnum right)
            where TEnum : FlagsEnumeration<TEnum>, new() => (left & right) == right;

        public static bool HasFlags<TEnum>(this FlagsEnumeration<TEnum> left, TEnum right)
            where TEnum : FlagsEnumeration<TEnum>, new() => (left & right) == right;

        public static bool ExistInFlags<TEnum>(this TEnum left, TEnum right)
            where TEnum : FlagsEnumeration<TEnum>, new() => (left & right) != new TEnum();

        public static bool ExistInFlags<TEnum>(this FlagsEnumeration<TEnum> left, TEnum right)
            where TEnum : FlagsEnumeration<TEnum>, new() => (left & right) != new TEnum();

        public static TEnum SetFlags<TEnum>(this TEnum left, params TEnum[] right)
            where TEnum : FlagsEnumeration<TEnum>, new() => right.Aggregate(left, (current, item) => current | item);

        public static TEnum UnsetFlags<TEnum>(this TEnum left, params TEnum[] right)
            where TEnum : FlagsEnumeration<TEnum>, new() => right.Aggregate(left, (current, item) => current & ~item);

        public static TEnum ToggleFlags<TEnum>(this TEnum left, params TEnum[] right)
            where TEnum : FlagsEnumeration<TEnum>, new() => right.Aggregate(left, (current, item) => current ^ item);

        public static PropertyBuilder<TEnum?> OwnFlagsEnumeration<TEntity, TEnum>([NotNull] this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TEnum?>> property)
            where TEntity : class
            where TEnum : FlagsEnumeration<TEnum>, new() => builder.Property(property).HasConversion(t => t!.ToUniqueId(), t => t.FromUniqueId<TEnum>());

        public static TEnum? ListToFlagsEnum<TEnum>([NotNull] this IEnumerable<string> names)
            where TEnum : FlagsEnumeration<TEnum>, new()
        {
            TEnum? enumeration = null;
            foreach (var name in names)
            {
                var item = name.FromName<TEnum>() ?? name.FromUniqueId<TEnum>();
                if (item is null)
                {
                    continue;
                }

                if (enumeration is null)
                {
                    enumeration = item;
                }
                else
                {
                    enumeration |= item;
                }
            }

            return enumeration;
        }

        public static TEnum? ListToFlagsEnum<TEnum>(this IEnumerable<TEnum>? value)
            where TEnum : FlagsEnumeration<TEnum>, new()
        {
            if (value is null)
            {
                return null;
            }

            TEnum? enumeration = null;
            foreach (var item in value)
            {
                if (enumeration is null)
                {
                    enumeration = item;
                }
                else
                {
                    enumeration |= item;
                }
            }

            return enumeration;
        }
    }
}
