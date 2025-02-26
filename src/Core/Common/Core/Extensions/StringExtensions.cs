namespace GamaEdtech.Common.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty([NotNullWhen(returnValue: false)] this string? str) => string.IsNullOrEmpty(str);

        public static string? Left(this string? str, int len) => str?.Length < len ? throw new ArgumentException("len argument can not be bigger than given string's length!") : (str?[..len]);

        public static string? Right([NotNull] this string str, int len) => str.Length < len
                ? throw new ArgumentException("len argument can not be bigger than given string's length!")
                : str.Substring(str.Length - len, len);

        public static string[]? Split(this string? str, string separator) => str?.Split([separator], StringSplitOptions.None);

        public static string[]? Split(this string? str, string separator, StringSplitOptions options) => str?.Split([separator], options);

        public static IEnumerable<ReadOnlyMemory<char>> Split(this ReadOnlyMemory<char> chars, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            int index;
            while ((index = chars.Span.IndexOf(separator)) >= 0)
            {
                var slice = chars[..index];
                if ((options & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries)
                {
                    slice = slice.Trim();
                }

                if ((options & StringSplitOptions.RemoveEmptyEntries) == 0 || slice.Length > 0)
                {
                    yield return slice;
                }

                chars = chars[(index + 1)..];
            }

            if ((options & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries)
            {
                chars = chars.Trim();
            }

            if ((options & StringSplitOptions.RemoveEmptyEntries) == 0 || chars.Length > 0)
            {
                yield return chars;
            }
        }

        public static string[]? SplitToLines(this string? str) => str?.Split(Environment.NewLine);

        public static string[]? SplitToLines(this string? str, StringSplitOptions options) => str?.Split(Environment.NewLine, options);

        public static T? ToEnum<T>(this string? value)
            where T : struct => string.IsNullOrEmpty(value) ? null : Enum.Parse<T>(value);

        public static T? ToEnum<T>(this string? value, bool ignoreCase)
            where T : struct => string.IsNullOrEmpty(value) ? null : Enum.Parse<T>(value, ignoreCase);

        public static byte[] GetBytes(this string str) => str.GetBytes(Encoding.UTF8);

        public static byte[] GetBytes([NotNull] this string str, [NotNull] Encoding encoding) => encoding.GetBytes(str);
    }
}
