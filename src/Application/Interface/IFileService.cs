namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Enumeration;

    [Injectable]
    public interface IFileService
    {
        ResultData<Uri?> GetFileUri(string? id, ContainerType containerType);
        Task<ResultData<string?>> UploadFileAsync([NotNull] UploadFileRequestDto requestDto);
        Task<ResultData<bool>> RemoveFileAsync([NotNull] RemoveFileRequestDto requestDto);
    }
}
