namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.TimeZone;

    using Microsoft.Extensions.DependencyInjection;

    using static GamaEdtech.Common.Core.Constants;

    public class DateTimeConverter<T> : JsonConverter<T>
    {
        private readonly string? format;
        private readonly FormatProvider formatProvider;

        public DateTimeConverter(string? format, FormatProvider? formatProvider)
        {
            this.format = format;
            this.formatProvider = formatProvider ?? FormatProvider.CurrentCulture;
        }

        public DateTimeConverter()
        {
        }

        public override bool HandleNull => true;

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, [NotNull] JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return (T?)(object?)null;
            }

            IFormatProvider provider = formatProvider == FormatProvider.CurrentCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;

            if (typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?))
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)dt;
                }

                var dateTime = reader.GetString().ValueOf<DateTime?>();
                return dateTime.HasValue ? (T?)(object?)dateTime.Value : (T?)(object?)null;
            }
            else if (typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?))
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)new DateTimeOffset(new DateTime(dt.Ticks, DateTimeKind.Unspecified), GetTimeSpan(options));
                }

                var dateTime = reader.GetString().ValueOf<DateTime?>();
                return dateTime.HasValue
                    ? (T?)(object?)new DateTimeOffset(new DateTime(dateTime.Value.Ticks, DateTimeKind.Unspecified), GetTimeSpan(options))
                    : (T?)(object?)null;
            }
            else if (typeToConvert == typeof(DateOnly) || typeToConvert == typeof(DateOnly?))
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)DateOnly.FromDateTime(dt);
                }

                var dateString = reader.GetString();
                var date = dateString.ValueOf<DateOnly?>();
                if (date.HasValue)
                {
                    return (T?)(object?)date;
                }

                var dateTime = dateString.ValueOf<DateTime?>();
                return dateTime.HasValue ? (T?)(object?)DateOnly.FromDateTime(dateTime.Value) : (T?)(object?)null;
            }
            else if (typeToConvert == typeof(TimeOnly) || typeToConvert == typeof(TimeOnly?))
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return (T?)(object?)TimeOnly.FromDateTime(dt);
                }

                var timeString = reader.GetString();
                var time = timeString.ValueOf<TimeOnly?>();
                if (time.HasValue)
                {
                    return (T?)(object?)time;
                }

                var dateTime = timeString.ValueOf<DateTime?>();
                if (dateTime.HasValue)
                {
                    return (T?)(object?)TimeOnly.FromDateTime(dateTime.Value);
                }

                var timeFormat = string.IsNullOrEmpty(format) ? "HH:mm:ss" : format;
                return !string.IsNullOrEmpty(timeString) && TimeOnly.TryParseExact(timeString, timeFormat, provider, DateTimeStyles.None, out var result)
                    ? (T?)(object?)result
                    : (T?)(object?)null;
            }

            throw new NotSupportedException();
        }

        public override void Write([NotNull] Utf8JsonWriter writer, T? value, [NotNull] JsonSerializerOptions options)
        {
            IFormatProvider provider = formatProvider == FormatProvider.CurrentCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;

            if (value is DateTimeOffset offset)
            {
                writer.WriteStringValue(offset.UtcDateTime.Add(GetTimeSpan(options)).ToString(format, provider));
            }
            else
            {
                writer.WriteStringValue((value as ISpanFormattable)?.ToString(format, provider));
            }
        }

        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(DateTime) ||
                typeToConvert == typeof(DateTime?) ||

                typeToConvert == typeof(DateTimeOffset) ||
                typeToConvert == typeof(DateTimeOffset?) ||

                typeToConvert == typeof(DateOnly) ||
                typeToConvert == typeof(DateOnly?) ||

                typeToConvert == typeof(TimeOnly) ||
                typeToConvert == typeof(TimeOnly?);

        private static TimeSpan GetTimeSpan(JsonSerializerOptions options)
        {
            var converter = options.Converters.OfType<ServiceProviderDummyConverter>().FirstOrDefault();
            var timeZoneId = converter?.HttpContextAccessor.HttpContext?.User.FindFirstValue(TimeZoneIdClaim) ?? UtcTimeZoneId;
            return converter?.ServiceProvider?.GetRequiredService<ITimeZoneProvider>().GetTimeZones()?.FirstOrDefault(t => t.Id == timeZoneId)?.BaseUtcOffset ?? BaseUtcOffset;
        }
    }
}
