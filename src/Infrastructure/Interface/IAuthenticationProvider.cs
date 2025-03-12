namespace GamaEdtech.Infrastructure.Interface
{
    using GamaEdtech.Common.Data;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.Identity;
    using GamaEdtech.Domain.Enumeration;

    [Injectable]
    public interface IAuthenticationProvider : IProvider<AuthenticationProvider>
    {
        Task<ResultData<AuthenticationResponseDto>> AuthenticateAsync([NotNull] AuthenticationRequestDto requestDto);
    }
}
