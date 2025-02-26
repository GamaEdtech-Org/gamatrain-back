namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.DataAnnotation;

    internal sealed class CustomtJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            var jsonTypeInfo = base.GetTypeInfo(type, options);

            var lst = jsonTypeInfo.Properties.Where(t => IsValidType(t.PropertyType));
            if (lst?.Any() == true)
            {
                foreach (var prop in lst)
                {
                    var formatProvider = Constants.FormatProvider.CurrentCulture;
                    string? dataFormatString = null;

                    var displayFormat = (prop.AttributeProvider as MemberInfo)?.GetCustomAttribute<DisplayFormatAttribute>();
                    if (displayFormat is not null)
                    {
                        formatProvider = displayFormat.FormatProvider;
                        dataFormatString = displayFormat.DataFormatString;
                    }

                    if (prop.PropertyType == typeof(DateTime))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateTime>(dataFormatString, formatProvider);
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateTime?>(dataFormatString, formatProvider);
                    }
                    else if (prop.PropertyType == typeof(DateTimeOffset))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateTimeOffset>(dataFormatString, formatProvider);
                    }
                    else if (prop.PropertyType == typeof(DateTimeOffset?))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateTimeOffset?>(dataFormatString, formatProvider);
                    }
                    else if (prop.PropertyType == typeof(DateOnly))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateOnly>(dataFormatString, formatProvider);
                    }
                    else if (prop.PropertyType == typeof(DateOnly?))
                    {
                        prop.CustomConverter = new DateTimeConverter<DateOnly?>(dataFormatString, formatProvider);
                    }
                    else if (prop.PropertyType == typeof(TimeOnly))
                    {
                        prop.CustomConverter = new DateTimeConverter<TimeOnly>(dataFormatString, formatProvider);
                    }
                    else if (prop.PropertyType == typeof(TimeOnly?))
                    {
                        prop.CustomConverter = new DateTimeConverter<TimeOnly?>(dataFormatString, formatProvider);
                    }
                }
            }

            return jsonTypeInfo;
        }

        private static bool IsValidType(Type type)
        {
            var lst = new[] { typeof(DateTime), typeof(DateTime?), typeof(DateTimeOffset), typeof(DateTimeOffset?), typeof(DateOnly), typeof(DateOnly?), typeof(TimeOnly), typeof(TimeOnly?) };
            return lst.Contains(type);
        }
    }
}
