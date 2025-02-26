namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    using GamaEdtech.Data.Dto.File;

    [Injectable]
    public interface IFileService
    {
        Task<ResultData<CreateFileResponseDto>> CreateFileAsync([NotNull] CreateFileRequestDto requestDto);
        ResultData<Uri?> GetFileUri(string? id);
    }
}
