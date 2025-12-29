namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Email;

    [Injectable]
    public interface IEmailService
    {
        Task<ResultData<Void>> SendEmailAsync([NotNull] SendEmailRequestDto requestDto);
    }
}
