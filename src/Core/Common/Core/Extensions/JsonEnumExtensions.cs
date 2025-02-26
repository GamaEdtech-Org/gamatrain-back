namespace GamaEdtech.Common.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.Json;

    using GamaEdtech.Common.Converter.Enum;

    public static class JsonEnumExtensions
    {
        public static bool TryGetEnumAttribute<TAttribute>([NotNull] Type type, string? name, [NotNullWhen(returnValue: true)] out TAttribute? attribute)
            where TAttribute : Attribute
        {
            if (name is null)
            {
                attribute = null;
                return false;
            }

            var member = type.GetMember(name).SingleOrDefault();
            attribute = member?.GetCustomAttribute<TAttribute>(false);
            return attribute is not null;
        }

        public static ulong ToUInt64<TEnum>(this TEnum value)
            where TEnum : struct, Enum
            => value.ToUInt64(Type.GetTypeCode(typeof(TEnum)));

        public static ulong ToUInt64<TEnum>(this TEnum value, TypeCode enumTypeCode)
            where TEnum : struct, Enum => enumTypeCode switch
            {
                TypeCode.SByte => unchecked((ulong)Unsafe.As<TEnum, sbyte>(ref value)),
                TypeCode.Int16 => unchecked((ulong)Unsafe.As<TEnum, short>(ref value)),
                TypeCode.Int32 => unchecked((ulong)Unsafe.As<TEnum, int>(ref value)),
                TypeCode.Int64 => unchecked((ulong)Unsafe.As<TEnum, long>(ref value)),
                TypeCode.Byte => Unsafe.As<TEnum, byte>(ref value),
                TypeCode.UInt16 => Unsafe.As<TEnum, ushort>(ref value),
                TypeCode.UInt32 => Unsafe.As<TEnum, uint>(ref value),
                TypeCode.UInt64 => Unsafe.As<TEnum, ulong>(ref value),
                _ => throw new ArgumentException(enumTypeCode.ToString()),
            };

        public static TEnum FromUInt64<TEnum>(this ulong value)
            where TEnum : struct, Enum
            => value.FromUInt64<TEnum>(Type.GetTypeCode(typeof(TEnum)));

        public static TEnum FromUInt64<TEnum>(this ulong value, TypeCode enumTypeCode)
            where TEnum : struct, Enum
        {
            switch (enumTypeCode)
            {
                case TypeCode.SByte:
                {
                    var i = unchecked((sbyte)value);
                    return Unsafe.As<sbyte, TEnum>(ref i);
                }

                case TypeCode.Int16:
                {
                    var i = unchecked((short)value);
                    return Unsafe.As<short, TEnum>(ref i);
                }

                case TypeCode.Int32:
                {
                    var i = unchecked((int)value);
                    return Unsafe.As<int, TEnum>(ref i);
                }

                case TypeCode.Int64:
                {
                    var i = unchecked((long)value);
                    return Unsafe.As<long, TEnum>(ref i);
                }

                case TypeCode.Byte:
                {
                    var i = unchecked((byte)value);
                    return Unsafe.As<byte, TEnum>(ref i);
                }

                case TypeCode.UInt16:
                {
                    var i = unchecked((ushort)value);
                    return Unsafe.As<ushort, TEnum>(ref i);
                }

                case TypeCode.UInt32:
                {
                    var i = unchecked((uint)value);
                    return Unsafe.As<uint, TEnum>(ref i);
                }

                case TypeCode.UInt64:
                {
                    var i = unchecked(value);
                    return Unsafe.As<ulong, TEnum>(ref i);
                }

                default:
                    throw new ArgumentException(enumTypeCode.ToString());
            }
        }

        // Return data about the enum sorted by the binary values of the enumeration constants (that is, by their unsigned magnitude)
        public static IEnumerable<EnumData<TEnum>> GetData<TEnum>(JsonNamingPolicy? namingPolicy, TryOverrideName? tryOverrideName)
            where TEnum : struct, Enum => GetData<TEnum>(namingPolicy, tryOverrideName, Type.GetTypeCode(typeof(TEnum)));

        // Return data about the enum sorted by the binary values of the enumeration constants (that is, by their unsigned magnitude)
        public static IEnumerable<EnumData<TEnum>> GetData<TEnum>(JsonNamingPolicy? namingPolicy, TryOverrideName? tryOverrideName, TypeCode enumTypeCode)
            where TEnum : struct, Enum
        {
            var names = Enum.GetNames<TEnum>();
            var values = Enum.GetValues<TEnum>();
            return names.Zip(values, (n, v) =>
            {
                if (tryOverrideName is null || !tryOverrideName(typeof(TEnum), n, out var jsonName))
                {
                    jsonName = namingPolicy is null ? n.AsMemory() : namingPolicy.ConvertName(n).AsMemory();
                }

                return new EnumData<TEnum>(jsonName, v, v.ToUInt64(enumTypeCode));
            });
        }

        public static ILookup<ReadOnlyMemory<char>, int> GetLookupTable<TEnum>([NotNull] EnumData<TEnum>[] namesAndValues)
            where TEnum : struct, Enum => Enumerable.Range(0, namesAndValues.Length).ToLookup(i => namesAndValues[i].Name, CharMemoryComparer.OrdinalIgnoreCase);

        public static bool TryLookupBest<TEnum>([NotNull] EnumData<TEnum>[] namesAndValues, [NotNull] ILookup<ReadOnlyMemory<char>, int> lookupTable, ReadOnlyMemory<char> name, out TEnum value)
            where TEnum : struct, Enum
        {
            var i = 0;
            var firstMatch = -1;
            foreach (var index in lookupTable[name])
            {
                if (firstMatch == -1)
                {
                    firstMatch = index;
                }
                else
                {
                    if (i == 1 && namesAndValues[firstMatch].Name.Span.Equals(name.Span, StringComparison.Ordinal))
                    {
                        value = namesAndValues[firstMatch].Value;
                        return true;
                    }

                    if (namesAndValues[index].Name.Span.Equals(name.Span, StringComparison.Ordinal))
                    {
                        value = namesAndValues[index].Value;
                        return true;
                    }
                }

                i++;
            }

            value = firstMatch == -1 ? default : namesAndValues[firstMatch].Value;
            return firstMatch != -1;
        }
    }
}
