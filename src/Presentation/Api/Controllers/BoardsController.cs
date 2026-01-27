namespace GamaEdtech.Presentation.Api.Controllers
{
    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Presentation.ViewModel.Board;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BoardsController(Lazy<ILogger<BoardsController>> logger, Lazy<IBoardService> boardService)
        : ApiControllerBase<BoardsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<IEnumerable<BoardsListResponseViewModel>>>()]
        public async Task<IActionResult<IEnumerable<BoardsListResponseViewModel>>> GetBoards()
        {
            try
            {
                var result = await boardService.Value.GetBoardsListAsync();
                return Ok<IEnumerable<BoardsListResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data is null
                    ? []
                    : result.Data.Select(t => new BoardsListResponseViewModel
                    {
                        Id = t.Id,
                        Code = t.Code,
                        Title = t.Title,
                        Icon = t.Icon
                    }),
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<IEnumerable<BoardsListResponseViewModel>>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
