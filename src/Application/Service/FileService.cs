namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Data.Dto.File;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;
    using GamaEdtech.Application.Interface;

#pragma warning disable CA1416 // Validate platform compatibility
    public class FileService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<FileService>> localizer
        , Lazy<ILogger<FileService>> logger, Lazy<IConfiguration> configuration)
        : LocalizableServiceBase<FileService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IFileService
    {
        public async Task<ResultData<CreateFileResponseDto>> CreateFileAsync([NotNull] CreateFileRequestDto requestDto)
        {
            try
            {
                using MemoryStream stream = new();
                await requestDto.File!.CopyToAsync(stream);
                var data = stream.ToArray();

                var (preview1, preview2, preview3) = GeneratePreview(data, requestDto.File.FileName);

                var fileId = await UploadFileAsync(data);
                var previewId1 = await UploadFileAsync(preview1);
                var previewId2 = await UploadFileAsync(preview2);
                var previewId3 = await UploadFileAsync(preview3);

                return new(OperationResult.Succeeded)
                {
                    Data = new()
                    {
                        FileId = fileId,
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

        public ResultData<Uri?> GetFileUri(string? id)
        {
            try
            {
                var connection = configuration.Value.GetValue<string>("Azure:ConnectionString");
                var container = configuration.Value.GetValue<string>("Azure:SchoolContainerName");

                var url = new Azure.Storage.Blobs.BlobClient(connection, container, id)
                    .GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(10));

                return new(OperationResult.Succeeded) { Data = url };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        private async Task<string?> UploadFileAsync(byte[]? file)
        {
            if (file is null)
            {
                return null;
            }

            var connection = configuration.Value.GetValue<string>("Azure:ConnectionString");
            var container = configuration.Value.GetValue<string>("Azure:ContainerName");
            var name = Guid.NewGuid().ToString("N");

            _ = await new Azure.Storage.Blobs.BlobClient(connection, container, name)
                .UploadAsync(new BinaryData(file));

            return name;
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
