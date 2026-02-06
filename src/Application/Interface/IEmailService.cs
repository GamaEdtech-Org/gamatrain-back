namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Email;

    using Microsoft.AspNetCore.Http;

    [Injectable]
    public interface IEmailService
    {
        Task<ResultData<Void>> SendEmailAsync([NotNull] SendEmailRequestDto requestDto);
        Task<ResultData<EmailDto>> ProccessInboundEmailAsync([NotNull] HttpRequest request);
    }
}
