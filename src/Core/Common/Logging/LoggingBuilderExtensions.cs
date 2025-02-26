namespace GamaEdtech.Common.Logging
{
    using Microsoft.Extensions.Logging;

    using Serilog;

    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddFilelog(this ILoggingBuilder builder) => builder.AddSerilog();
    }
}
