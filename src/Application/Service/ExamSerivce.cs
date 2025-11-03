namespace GamaEdtech.Application.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.Game;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using HandlebarsDotNet;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public partial class ExamSerivce(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor,
        Lazy<IStringLocalizer<ExamSerivce>> localizer, Lazy<ILogger<ExamSerivce>> logger, Lazy<ICoreProvider> coreProvider
        , Lazy<IWebHostEnvironment> environment)
        : LocalizableServiceBase<ExamSerivce>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IExamService
    {
        public async Task<ResultData<ExportExamResponseDto>> ExportExamAsync([NotNull] ExportExamRequestDto requestDto)
        {
            try
            {
                var info = await coreProvider.Value.GetExamInformationAsync(new()
                {
                    ExamId = requestDto.ExamId,
                    SecretKey = requestDto.SecretKey,
                });
                if (info.OperationResult is not OperationResult.Succeeded)
                {
                    return new(info.OperationResult) { Errors = info.Errors };
                }

                if (info.Data is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["ExamNotFound"] },] };
                }

                info.Data.Url = requestDto.Url;
                if (requestDto.Duration.HasValue)
                {
                    info.Data.Exam!.ExamTime = requestDto.Duration.ToString();
                }

                byte[]? content = null;
                if (requestDto.FileType == ExportFileType.Pdf)
                {
                    content = await ExportPdfAsync();
                }
                else if (requestDto.FileType == ExportFileType.Word)
                {
                    content = await ExportDocumentAsync();
                }
                else if (requestDto.FileType == ExportFileType.PowerPoint)
                {
                    content = await ExportPresentationAsync();
                }

                return new(OperationResult.Succeeded)
                {
                    Data = new()
                    {
                        Content = content,
                    },
                };

                async Task<byte[]> ExportPdfAsync()
                {
                    var file = Path.Combine(environment.Value.WebRootPath, "exam.html");
                    var templateContent = await File.ReadAllTextAsync(file);

                    var template = Handlebars.Compile(templateContent);
                    var html = template(info.Data);
                    return Freeware.Html2Pdf.Convert(html);
                }

                async Task<byte[]> ExportDocumentAsync()
                {
                    var file = Path.Combine(environment.Value.WebRootPath, "exam.docx.html");
                    var templateContent = await File.ReadAllTextAsync(file);

                    if (info.Data.Tests is not null)
                    {
                        for (var i = 0; i < info.Data.Tests.Count; i++)
                        {
                            var test = info.Data.Tests[i];
                            test.Question = $"{i + 1}- {string.Join("<br>", TextRegex().Matches(test.Question!).Select(t => t.Groups.Values.LastOrDefault()))}";
                            test.OptionA = string.Join("<br>", TextRegex().Matches(test.OptionA!).Select(t => t.Groups.Values.LastOrDefault()));
                            test.OptionB = string.Join("<br>", TextRegex().Matches(test.OptionB!).Select(t => t.Groups.Values.LastOrDefault()));
                            test.OptionC = string.Join("<br>", TextRegex().Matches(test.OptionC!).Select(t => t.Groups.Values.LastOrDefault()));
                            test.OptionD = string.Join("<br>", TextRegex().Matches(test.OptionD!).Select(t => t.Groups.Values.LastOrDefault()));
                        }
                    }

                    var template = Handlebars.Compile(templateContent);
                    var html = template(info.Data);

                    using var doc = new Spire.Doc.Document();
                    var section = doc.AddSection();
                    var paragraph = section.AddParagraph();
                    paragraph.AppendHTML(html);

                    using MemoryStream stream = new();
                    doc.SaveToStream(stream, Spire.Doc.FileFormat.Docx);
                    return stream.ToArray();
                }

                async Task<byte[]> ExportPresentationAsync()
                {
                    using var presentation = new Spire.Presentation.Presentation();

                    var header = Path.Combine(environment.Value.WebRootPath, "exam.header.html");
                    var headerContent = await File.ReadAllTextAsync(header);

                    var headerTemplate = Handlebars.Compile(headerContent);
                    var headerHtml = headerTemplate(info.Data);

                    var shapes = presentation.Slides[0].Shapes;
                    shapes.AddFromHtml(headerHtml);

                    if (info.Data.Tests is not null)
                    {
                        var item = Path.Combine(environment.Value.WebRootPath, "exam.item.html");
                        var itemContent = await File.ReadAllTextAsync(item);

                        var itemTemplate = Handlebars.Compile(itemContent);

                        foreach (var test in info.Data.Tests)
                        {
                            var slide = presentation.Slides.Append();
                            var itemHtml = itemTemplate(test);
                            slide.Shapes.AddFromHtml(itemHtml);
                        }
                    }


                    using MemoryStream stream = new();
                    presentation.SaveToFile(stream, Spire.Presentation.FileFormat.Pptx2013);
                    return stream.ToArray();
                }
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        [GeneratedRegex("<p>([^<]*)<\\/p>")]
        private static partial Regex TextRegex();
    }
}
