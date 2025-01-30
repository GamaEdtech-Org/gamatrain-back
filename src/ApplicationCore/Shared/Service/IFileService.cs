namespace GamaEdtech.Backend.Shared.Service
{
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    using GamaEdtech.Backend.Data.Dto.File;

    [Injectable]
    public interface IFileService
    {
        Task<ResultData<CreateFileResponseDto>> CreateFileAsync([NotNull] CreateFileRequestDto requestDto);
    }
}
