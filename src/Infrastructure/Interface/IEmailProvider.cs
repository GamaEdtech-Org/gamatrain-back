namespace GamaEdtech.Infrastructure.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.Email;
    using GamaEdtech.Domain.Enumeration;

    [Injectable]
    public interface IEmailProvider : IProvider<EmailProviderType>
    {
        Task<ResultData<Void>> SendEmailAsync([NotNull] EmailRequestDto requestDto);
    }
}
