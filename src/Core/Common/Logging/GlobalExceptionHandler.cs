namespace GamaEdtech.Common.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;

    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync([NotNull] HttpContext httpContext, [NotNull] Exception exception, CancellationToken cancellationToken)
        {
            logger.LogException(exception);

            httpContext.Response.StatusCode = StatusCodes.Status200OK;
            await httpContext.Response.WriteAsJsonAsync(
                new ApiResponse<object>
                {
                    Errors = [new Error { Message = exception.Message, }],
                }, cancellationToken);

            return true;
        }
    }
}
