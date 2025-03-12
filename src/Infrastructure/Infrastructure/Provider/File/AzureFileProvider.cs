namespace GamaEdtech.Infrastructure.Provider.File
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class AzureFileProvider(Lazy<ILogger<AzureFileProvider>> logger, Lazy<IConfiguration> configuration) : IFileProvider
    {
        public FileProviderType ProviderType => FileProviderType.Azure;

        public ResultData<Uri?> GetFileUri(string? id, ContainerType containerType)
        {
            try
            {
                var connection = configuration.Value.GetValue<string>("Azure:ConnectionString");

                var key = "Azure:ContainerName";
                if (containerType == ContainerType.School)
                {
                    key = "Azure:SchoolContainerName";
                }
                var container = configuration.Value.GetValue<string>(key);

                var url = new Azure.Storage.Blobs.BlobClient(connection, container, id)
                    .GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(10));

                return new(OperationResult.Succeeded) { Data = url };
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
                var name = Guid.NewGuid().ToString("N");
                var connection = configuration.Value.GetValue<string>("Azure:ConnectionString");

                var key = requestDto.ContainerType == ContainerType.School ? "Azure:SchoolContainerName" : "Azure:ContainerName";
                var container = configuration.Value.GetValue<string>(key);

                _ = await new Azure.Storage.Blobs.BlobClient(connection, container, name)
                    .UploadAsync(new BinaryData(requestDto.File));

                return new(OperationResult.Succeeded) { Data = name };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

    }
}
