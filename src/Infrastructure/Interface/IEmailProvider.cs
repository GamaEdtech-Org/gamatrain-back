namespace GamaEdtech.Infrastructure.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.Email;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;

    [Injectable]
    public interface IEmailProvider : IProvider<EmailProviderType>
    {
        Task<ResultData<Void>> SendEmailAsync([NotNull] EmailRequestDto requestDto);
        Task<ResultData<EmailDto>> ProccessInboundEmailAsync([NotNull] HttpRequest request);
    }
}
