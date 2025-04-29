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

        public ResultData<Uri?> GetFileUri(string? id, [NotNull] ContainerType containerType)
        {
            try
            {
                var path = $"{GetDirectoryPath(containerType, false)}/{id}";
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
                var dir = GetDirectoryPath(requestDto.ContainerType, true);
                if (!Directory.Exists(dir))
                {
                    _ = Directory.CreateDirectory(dir);
                }

                var value = $"{Guid.NewGuid():N}{requestDto.FileExtension}";
                var savedFilePath = Path.Combine(dir, value);

                await System.IO.File.WriteAllBytesAsync(savedFilePath, requestDto.File);

                return new(OperationResult.Succeeded) { Data = value };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> RemoveFileAsync([NotNull] RemoveFileRequestDto requestDto)
        {
            try
            {
                var file = Path.Combine(GetDirectoryPath(requestDto.ContainerType, true), requestDto.FileId);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.SetAttributes(file, FileAttributes.Normal);
                    System.IO.File.Delete(file);
                }

                return new(OperationResult.Succeeded) { Data = await Task.FromResult(true) };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        private string GetDirectoryPath([NotNull] ContainerType containerType, bool physicalPath)
        {
            var path = configuration.Value.GetValue<string>("FileProvider:Local:Path")!;
            if (physicalPath)
            {
                List<string> lst = [environment.Value.WebRootPath];
                lst.AddRange(path.Split('/'));
                lst.Add(containerType.Name);
                return Path.Combine([.. lst]);
            }

            var hostUrl = $"{httpContextAccessor.Value.HttpContext?.Request.Scheme}://{httpContextAccessor.Value.HttpContext?.Request.Host}";
            return hostUrl + "/" + path + "/" + containerType.Name;
        }
    }
}
