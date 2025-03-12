namespace GamaEdtech.Infrastructure.Provider.File
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class LocalFileProvider(Lazy<ILogger<LocalFileProvider>> logger
        , Lazy<IConfiguration> configuration, Lazy<IWebHostEnvironment> environment, Lazy<HttpContextAccessor> httpContextAccessor) : IFileProvider
    {
        public FileProviderType ProviderType => FileProviderType.Local;

        public ResultData<Uri?> GetFileUri(string? id, ContainerType containerType)
        {
            try
            {
                var hostUrl = $"{httpContextAccessor.Value.HttpContext?.Request.Scheme}://{httpContextAccessor.Value.HttpContext?.Request.Host}";
                var path = $"{hostUrl}/{configuration.Value.GetValue<string>("FileProvider:Local:Path")}/{id}";
                return new(OperationResult.Succeeded) { Data = new Uri(path) };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<string?>> UploadFileAsync([NotNull] UploadFileRequestDto requestDto)
        {
            try
            {
                var path = configuration.Value.GetValue<string>("FileProvider:Local:Path");
                var dir = Path.Combine(environment.Value.WebRootPath, path!.Replace("/", "\\", StringComparison.OrdinalIgnoreCase), requestDto.ContainerType.Name);
                if (!Directory.Exists(dir))
                {
                    _ = Directory.CreateDirectory(dir);
                }

                var value = $"{Guid.NewGuid():N}{requestDto.FileExtension}";
                var savedFilePath = Path.Combine(dir, value);

                await System.IO.File.WriteAllBytesAsync(savedFilePath, requestDto.File);

                return new(OperationResult.Succeeded) { Data = $"{requestDto.ContainerType.Name}/{value}" };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }
    }
}
