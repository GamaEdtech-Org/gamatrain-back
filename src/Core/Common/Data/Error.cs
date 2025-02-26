namespace GamaEdtech.Common.Data
{
    using System;
    using System.Text.RegularExpressions;

    using GamaEdtech.Common.Core;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public partial struct Error(Exception? exception)
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public static readonly Regex CodeRegex = CodeRegexPartial();

        private string? message = exception?.Message;

        public string? Message
        {
            readonly get => message;

            set
            {
                if (value is null)
                {
                    message = value;
                    return;
                }

                var match = CodeRegex.Match(value);
                if (match.Success)
                {
                    message = value.Replace(match.Value, string.Empty, StringComparison.OrdinalIgnoreCase);
                    Code = Constants.ErrorCodePrefix + match.Value.Replace("*", string.Empty, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    message = value;
                }
            }
        }

        public string? Code { get; set; }

        public string? Reference { get; set; }

        public string? Info { get; set; }

        public object? Value { get; set; }

        public static string FormatMessage(string? msg)
        {
            if (msg is null)
            {
                return string.Empty;
            }

            var match = CodeRegex.Match(msg);
            return match.Success ? msg.Replace(match.Value, string.Empty, StringComparison.OrdinalIgnoreCase) : msg;
        }

        [GeneratedRegex("\\*\\*\\d{3}\\*\\*")]
        private static partial Regex CodeRegexPartial();
    }
}
