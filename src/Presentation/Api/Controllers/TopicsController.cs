namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;

    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Specification.Topic;
    using GamaEdtech.Presentation.ViewModel.Topic;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TopicsController(Lazy<ILogger<TopicsController>> logger, Lazy<ITopicService> boardService)
        : ApiControllerBase<TopicsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<TopicsResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<TopicsResponseViewModel>>> GetTopics([NotNull, FromQuery] TopicsRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.GetTopicsAsync(new ListRequestDto<Topic>
                {
                    PagingDto = request.PagingDto,
                    Specification = request.SubjectId.HasValue ? new SubjectIdEqualsSpecification(request.SubjectId.Value) : null,
                });
                return Ok<ListDataSource<TopicsResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new TopicsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Order = t.Order,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ListDataSource<TopicsResponseViewModel>>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
