namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.EmailMarketing;

    [Injectable]
    public interface IEmailMarketingService
    {
        Task<ResultData<Void>> SendEmailAsync([NotNull] SendEmailRequestDto requestDto);
    }
}
