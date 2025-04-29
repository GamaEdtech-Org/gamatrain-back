namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Presentation.ViewModel.Board;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BoardsController(Lazy<ILogger<BoardsController>> logger, Lazy<IBoardService> boardService)
        : ApiControllerBase<BoardsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<BoardsResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<BoardsResponseViewModel>>> GetBoards([NotNull, FromQuery] BoardsRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.GetBoardsAsync(new ListRequestDto<Board>
                {
                    PagingDto = request.PagingDto,
                });
                return Ok<ListDataSource<BoardsResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new BoardsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Icon = t.Icon,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ListDataSource<BoardsResponseViewModel>>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
