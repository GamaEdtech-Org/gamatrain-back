namespace GamaEdtech.Infrastructure.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Enumeration;

    [Injectable]
    public interface IFileProvider : IProvider<FileProviderType>
    {
        ResultData<Uri?> GetFileUri(string? id, ContainerType containerType);
        Task<ResultData<string?>> UploadFileAsync([NotNull] UploadFileRequestDto requestDto);
    }
}
