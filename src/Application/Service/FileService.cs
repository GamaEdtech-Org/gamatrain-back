namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.File;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

#pragma warning disable CA1416 // Validate platform compatibility
    public class FileService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<FileService>> localizer
        , Lazy<ILogger<FileService>> logger, Lazy<IConfiguration> configuration, Lazy<IGenericFactory<IFileProvider, FileProviderType>> genericFactory)
        : LocalizableServiceBase<FileService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IFileService
    {
        private IFileProvider FileProvider
        {
            get
            {
                _ = configuration.Value.GetValue<string?>("FileProvider:Type").TryGetFromNameOrValue<FileProviderType, byte>(out var fileProviderType);
                return genericFactory.Value.GetProvider(fileProviderType!)!;
            }
        }

        public async Task<ResultData<CreateFileResponseDto>> CreateFileWithPreviewAsync([NotNull] CreateFileRequestDto requestDto)
        {
            try
            {
                using MemoryStream stream = new();
                await requestDto.File.CopyToAsync(stream);
                var data = stream.ToArray();

                var (preview1, preview2, preview3) = GeneratePreview(data, requestDto.File.FileName);

                var fileId = await UploadFileAsync(new() { File = data, ContainerType = ContainerType.Default, FileExtension = Path.GetExtension(requestDto.File.FileName) });
                var previewId1 = preview1 is not null ? (await UploadFileAsync(new() { File = preview1, ContainerType = ContainerType.Default, FileExtension = ".png" })).Data : null;
                var previewId2 = preview2 is not null ? (await UploadFileAsync(new() { File = preview2, ContainerType = ContainerType.Default, FileExtension = ".png" })).Data : null;
                var previewId3 = preview3 is not null ? (await UploadFileAsync(new() { File = preview3, ContainerType = ContainerType.Default, FileExtension = ".png" })).Data : null;

                return new(OperationResult.Succeeded)
                {
                    Data = new()
                    {
                        FileId = fileId.Data,
                        PreviewId1 = previewId1,
                        PreviewId2 = previewId2,
                        PreviewId3 = previewId3,
                    },
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public ResultData<Uri?> GetFileUri(string? id, ContainerType containerType)
        {
            try
            {
                return id is null ? new(OperationResult.Succeeded) { Data = null } : FileProvider.GetFileUri(id, containerType);
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<string?>> UploadFileAsync([NotNull] UploadFileRequestDto requestDto)
        {
            try
            {
                return await FileProvider.UploadFileAsync(requestDto);
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> RemoveFileAsync([NotNull] RemoveFileRequestDto requestDto)
        {
            try
            {
                return await FileProvider.RemoveFileAsync(requestDto);
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        private static (byte[]? PreviewId1, byte[]? PreviewId2, byte[]? PreviewId3) GeneratePreview(byte[] file, string fileName)
        {
            switch (Path.GetExtension(fileName).ToLowerInvariant())
            {
                case ".pdf":
                {
                    Image? img1 = null;
                    Image? img2 = null;
                    Image? img3 = null;
                    try
                    {
                        img1 = Image.FromStream(new MemoryStream(Freeware.Pdf2Png.Convert(file, 1, 100)));
                    }
                    catch
                    {
                        //ignore
                    }

                    if (img1 is not null)
                    {
                        try
                        {
                            img2 = Image.FromStream(new MemoryStream(Freeware.Pdf2Png.Convert(file, 2, 100)));
                        }
                        catch
                        {
                            //ignore
                        }
                    }

                    if (img2 is not null)
                    {
                        try
                        {
                            img3 = Image.FromStream(new MemoryStream(Freeware.Pdf2Png.Convert(file, 3, 100)));
                        }
                        catch
                        {
                            //ignore
                        }
                    }

                    var preview1 = ResizeImage(img1, 496, 702);
                    var preview2 = ResizeImage(img2, 496, 702);
                    var preview3 = ResizeImage(img3, 496, 702);

                    return (preview1, preview2, preview3);
                }

                case ".ppt":
                case ".pptx":
                {
                    using MemoryStream stream = new(file);
                    using var doc = new Spire.Presentation.Presentation();
                    doc.LoadFromStream(stream, Spire.Presentation.FileFormat.Ppsx2013);
                    var img1 = doc.Slides.Count > 0 ? doc.Slides[0].SaveAsImage(800, 450) : null;
                    var img2 = doc.Slides.Count > 1 ? doc.Slides[1].SaveAsImage(800, 450) : null;
                    var img3 = doc.Slides.Count > 2 ? doc.Slides[2].SaveAsImage(800, 450) : null;

                    var preview1 = ResizeImage(img1, 800, 450);
                    var preview2 = ResizeImage(img2, 800, 450);
                    var preview3 = ResizeImage(img3, 800, 450);

                    return (preview1, preview2, preview3);
                }

                case ".doc":
                case ".docx":
                {
                    using MemoryStream stream = new(file);
                    using var doc = new Spire.Doc.Document();
                    doc.LoadFromStream(stream, Spire.Doc.FileFormat.Docx2013);
                    var imgs = doc.SaveToImages(0, 3, Spire.Doc.Documents.ImageType.Bitmap);

                    var preview1 = imgs?.Length > 0 ? ResizeImage(imgs[0], 496, 702) : null;
                    var preview2 = imgs?.Length > 1 ? ResizeImage(imgs[1], 496, 702) : null;
                    var preview3 = imgs?.Length > 2 ? ResizeImage(imgs[2], 496, 702) : null;

                    return (preview1, preview2, preview3);
                }
                default:
                    throw new NotSupportedException(fileName);
            }
        }

        private static byte[]? ResizeImage(Image? image, int width, int height)
        {
            if (image is null)
            {
                return null;
            }

            var destRect = new Rectangle(0, 0, width, height);
            using var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using ImageAttributes wrapMode = new();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            using MemoryStream stream = new();
            destImage.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
    }
#pragma warning restore CA1416 // Validate platform compatibility
}
