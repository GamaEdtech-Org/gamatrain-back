namespace GamaEdtech.Infrastructure.Provider.File
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.YouTube.v3;
    using Google.Apis.YouTube.v3.Data;

    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class YoutubeFileProvider(Lazy<ILogger<YoutubeFileProvider>> logger) : IFileProvider
    {
        public FileProviderType ProviderType => FileProviderType.Youtube;

        public ResultData<Uri?> GetFileUri(string id, ContainerType containerType) => throw new NotImplementedException();
        public Task<ResultData<bool>> RemoveFileAsync([NotNull] RemoveFileRequestDto requestDto) => throw new NotImplementedException();

        public async Task<ResultData<string?>> UploadFileAsync([NotNull] UploadFileRequestDto requestDto)
        {
            try
            {
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                {
                    ClientId = "YOUR_CLIENT_ID",
                    ClientSecret = "YOUR_CLIENT_SECRET",
                }, new[] { YouTubeService.Scope.YoutubeUpload }, "user", CancellationToken.None);

                using var youtubeService = new YouTubeService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name,
                });

                var video = new Video
                {
                    Snippet = new()
                    {
                        Title = "Default Video Title",
                        Description = "Default Video Description",
                        Tags = new string[] { "tag1", "tag2" },
                        CategoryId = "22", // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                    },
                    Status = new VideoStatus
                    {
                        PrivacyStatus = "unlisted" // or "private" or "public"
                    }
                };

                var filePath = @"REPLACE_ME.mp4"; // Replace with path to actual movie file.

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");

                    var fdfdf = await videosInsertRequest.UploadAsync();
                    _ = fdfdf.Exception;
                }

                var name = $"{Guid.NewGuid():N}{requestDto.FileExtension}";

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
