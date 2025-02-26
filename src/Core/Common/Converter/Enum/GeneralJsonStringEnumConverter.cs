namespace GamaEdtech.Common.Converter.Enum
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Core.Extensions;
    using GamaEdtech.Common.Core.Extensions.Collections;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    public readonly record struct EnumData<TEnum>(ReadOnlyMemory<char> Name, TEnum Value, ulong UInt64Value)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        where TEnum : struct, Enum;

    public delegate bool TryOverrideName(Type enumType, string? name, out ReadOnlyMemory<char> overrideName);

    public class GeneralJsonStringEnumConverter : JsonConverterFactory
    {
        private readonly JsonNamingPolicy? namingPolicy;
        private readonly bool allowIntegerValues;

        public GeneralJsonStringEnumConverter()
            : this(null, true)
        {
        }

        public GeneralJsonStringEnumConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues) => (this.namingPolicy, this.allowIntegerValues) = (namingPolicy, allowIntegerValues);

        public override bool CanConvert([NotNull] Type typeToConvert) => typeToConvert.IsEnum || Nullable.GetUnderlyingType(typeToConvert)?.IsEnum == true;

        public sealed override JsonConverter? CreateConverter([NotNull] Type typeToConvert, JsonSerializerOptions options)
        {
            var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            var flagged = enumType.IsDefined(typeof(FlagsAttribute), true);
            JsonConverter enumConverter;
            TryOverrideName tryOverrideName = TryOverrideName;
            var converterType = (flagged ? typeof(FlaggedJsonEnumConverter<>) : typeof(UnflaggedJsonEnumConverter<>)).MakeGenericType(enumType);
            enumConverter = (JsonConverter)Activator.CreateInstance(
                converterType,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                args: [namingPolicy!, allowIntegerValues, tryOverrideName],
                culture: null)!;
            if (enumType == typeToConvert)
            {
                return enumConverter;
            }
            else
            {
                var nullableConverter = (JsonConverter)Activator.CreateInstance(
                    typeof(NullableConverterDecorator<>).MakeGenericType(enumType),
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    binder: null,
                    args: [enumConverter],
                    culture: null)!;
                return nullableConverter;
            }
        }

        protected virtual bool TryOverrideName(Type enumType, string? name, out ReadOnlyMemory<char> overrideName)
        {
            overrideName = default;
            return false;
        }

#pragma warning disable CA1034 // Nested types should not be visible
        public class FlaggedJsonEnumConverter<TEnum>(JsonNamingPolicy? namingPolicy, bool allowNumbers, TryOverrideName? tryOverrideName) : JsonEnumConverterBase<TEnum>(namingPolicy, allowNumbers, tryOverrideName)
            where TEnum : struct, Enum
        {
            private const char FlagSeparatorChar = ',';
            private const string FlagSeparatorString = ", ";

            protected override bool TryFormatAsString([NotNull] EnumData<TEnum>[] enumData, TEnum value, out ReadOnlyMemory<char> name)
            {
                var uInt64Value = value.ToUInt64(EnumTypeCode);
                var index = enumData.BinarySearchFirst(uInt64Value, EntryComparer);
                if (index >= 0)
                {
                    // A single flag
                    name = enumData[index].Name;
                    return true;
                }

                if (uInt64Value != 0)
                {
                    StringBuilder? sb = null;
                    for (var i = ~index - 1; i >= 0; i--)
                    {
                        if ((uInt64Value & enumData[i].UInt64Value) == enumData[i].UInt64Value && enumData[i].UInt64Value != 0)
                        {
                            if (sb is null)
                            {
                                sb = new StringBuilder();
                                _ = sb.Append(enumData[i].Name.Span);
                            }
                            else
                            {
                                _ = sb.Insert(0, FlagSeparatorString);
                                _ = sb.Insert(0, enumData[i].Name.Span);
                            }

                            uInt64Value -= enumData[i].UInt64Value;
                        }
                    }

                    if (uInt64Value == 0 && sb is not null)
                    {
                        name = sb.ToString().AsMemory();
                        return true;
                    }
                }

                name = default;
                return false;
            }

            protected override bool TryReadAsString(EnumData<TEnum>[] enumData, ILookup<ReadOnlyMemory<char>, int> nameLookup, ReadOnlyMemory<char> name, out TEnum value)
            {
                ulong uInt64Value = 0;
                foreach (var slice in name.Split(FlagSeparatorChar, StringSplitOptions.TrimEntries))
                {
                    if (JsonEnumExtensions.TryLookupBest(enumData, nameLookup, slice, out var thisValue))
                    {
                        uInt64Value |= thisValue.ToUInt64(EnumTypeCode);
                    }
                    else
                    {
                        value = default;
                        return false;
                    }
                }

                value = uInt64Value.FromUInt64<TEnum>();
                return true;
            }
        }

        public class UnflaggedJsonEnumConverter<TEnum>(JsonNamingPolicy? namingPolicy, bool allowNumbers, TryOverrideName? tryOverrideName) : JsonEnumConverterBase<TEnum>(namingPolicy, allowNumbers, tryOverrideName)
            where TEnum : struct, Enum
        {
            protected override bool TryFormatAsString([NotNull] EnumData<TEnum>[] enumData, TEnum value, out ReadOnlyMemory<char> name)
            {
                var index = enumData.BinarySearchFirst(value.ToUInt64(EnumTypeCode), EntryComparer);
                if (index >= 0)
                {
                    name = enumData[index].Name;
                    return true;
                }

                name = default;
                return false;
            }

            protected override bool TryReadAsString(EnumData<TEnum>[] enumData, ILookup<ReadOnlyMemory<char>, int> nameLookup, ReadOnlyMemory<char> name, out TEnum value) =>
                JsonEnumExtensions.TryLookupBest(enumData, nameLookup, name, out value);
        }

        public abstract class JsonEnumConverterBase<TEnum> : JsonConverter<TEnum>
            where TEnum : struct, Enum
        {
            protected JsonEnumConverterBase(JsonNamingPolicy? namingPolicy, bool allowNumbers, TryOverrideName? tryOverrideName)
            {
                AllowNumbers = allowNumbers;
                EnumData = [.. JsonEnumExtensions.GetData<TEnum>(namingPolicy, tryOverrideName)];
                NameLookup = JsonEnumExtensions.GetLookupTable(EnumData);
            }

            protected static TypeCode EnumTypeCode { get; } = Type.GetTypeCode(typeof(TEnum));

            protected static Func<EnumData<TEnum>, ulong, int> EntryComparer { get; } = (item, key) => item.UInt64Value.CompareTo(key);

            private bool AllowNumbers { get; }

            private EnumData<TEnum>[] EnumData { get; }

            private ILookup<ReadOnlyMemory<char>, int> NameLookup { get; }

            public sealed override void Write([NotNull] Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/src/System/Text/Json/Serialization/Converters/Value/EnumConverter.cs
                if (TryFormatAsString(EnumData, value, out var name))
                {
                    writer.WriteStringValue(name.Span);
                }
                else
                {
                    if (!AllowNumbers)
                    {
                        throw new JsonException();
                    }

                    WriteEnumAsNumber(writer, value);
                }
            }

            public sealed override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                reader.TokenType switch
                {
                    JsonTokenType.String => TryReadAsString(EnumData, NameLookup, reader.GetString().AsMemory(), out var value) ? value : throw new JsonException(),
                    JsonTokenType.Number => AllowNumbers ? ReadNumberAsEnum(ref reader) : throw new JsonException(),
                    _ => throw new JsonException(),
                };

            protected abstract bool TryFormatAsString(EnumData<TEnum>[] enumData, TEnum value, out ReadOnlyMemory<char> name);

            protected abstract bool TryReadAsString(EnumData<TEnum>[] enumData, ILookup<ReadOnlyMemory<char>, int> nameLookup, ReadOnlyMemory<char> name, out TEnum value);

            private static void WriteEnumAsNumber(Utf8JsonWriter writer, TEnum value)
            {
                switch (EnumTypeCode)
                {
                    case TypeCode.SByte:
                        writer.WriteNumberValue(Unsafe.As<TEnum, sbyte>(ref value));
                        break;
                    case TypeCode.Int16:
                        writer.WriteNumberValue(Unsafe.As<TEnum, short>(ref value));
                        break;
                    case TypeCode.Int32:
                        writer.WriteNumberValue(Unsafe.As<TEnum, int>(ref value));
                        break;
                    case TypeCode.Int64:
                        writer.WriteNumberValue(Unsafe.As<TEnum, long>(ref value));
                        break;
                    case TypeCode.Byte:
                        writer.WriteNumberValue(Unsafe.As<TEnum, byte>(ref value));
                        break;
                    case TypeCode.UInt16:
                        writer.WriteNumberValue(Unsafe.As<TEnum, ushort>(ref value));
                        break;
                    case TypeCode.UInt32:
                        writer.WriteNumberValue(Unsafe.As<TEnum, uint>(ref value));
                        break;
                    case TypeCode.UInt64:
                        writer.WriteNumberValue(Unsafe.As<TEnum, ulong>(ref value));
                        break;
                    default:
                        throw new JsonException();
                }
            }

            private static TEnum ReadNumberAsEnum(ref Utf8JsonReader reader)
            {
                switch (EnumTypeCode)
                {
                    case TypeCode.SByte:
                    {
                        var i = reader.GetSByte();
                        return Unsafe.As<sbyte, TEnum>(ref i);
                    }

                    case TypeCode.Int16:
                    {
                        var i = reader.GetInt16();
                        return Unsafe.As<short, TEnum>(ref i);
                    }

                    case TypeCode.Int32:
                    {
                        var i = reader.GetInt32();
                        return Unsafe.As<int, TEnum>(ref i);
                    }

                    case TypeCode.Int64:
                    {
                        var i = reader.GetInt64();
                        return Unsafe.As<long, TEnum>(ref i);
                    }

                    case TypeCode.Byte:
                    {
                        var i = reader.GetByte();
                        return Unsafe.As<byte, TEnum>(ref i);
                    }

                    case TypeCode.UInt16:
                    {
                        var i = reader.GetUInt16();
                        return Unsafe.As<ushort, TEnum>(ref i);
                    }

                    case TypeCode.UInt32:
                    {
                        var i = reader.GetUInt32();
                        return Unsafe.As<uint, TEnum>(ref i);
                    }

                    case TypeCode.UInt64:
                    {
                        var i = reader.GetUInt64();
                        return Unsafe.As<ulong, TEnum>(ref i);
                    }

                    default:
                        throw new JsonException();
                }
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
